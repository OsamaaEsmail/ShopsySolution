using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Errors;
using Catalog.Infrastructure.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Infrastructure.Services;

public class ProductService(CatalogDbContext context, IMapper mapper, ILogger<ProductService> logger) : IProductService
{
    public async Task<Result<ProductResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        logger.LogInformation("Getting product by ID: {ProductId}", id);

        var product = await context.Products
            .Include(p => p.Images)
            .Include(p => p.Stocks)
            .Include(p => p.Category)
            .Include(p => p.SubCategory)
            .Include(p => p.Vendor)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

        if (product is null)
        {
            logger.LogWarning("Product not found: {ProductId}", id);
            return Result.Failure<ProductResponse>(ProductErrors.ProductNotFound);
        }

        return Result.Success(mapper.Map<ProductResponse>(product));
    }

    public async Task<Result<PaginatedList<ProductResponse>>> GetAllAsync(int pageNumber, int pageSize, Guid? categoryId = null, CancellationToken ct = default)
    {
        logger.LogInformation("Getting products, Page: {PageNumber}, Size: {PageSize}, CategoryFilter: {CategoryId}", pageNumber, pageSize, categoryId);

        var query = context.Products
            .Include(p => p.Images)
            .Include(p => p.Stocks)
            .Include(p => p.Category)
            .Include(p => p.SubCategory)
            .Include(p => p.Vendor)
            .AsNoTracking()
            .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        var totalCount = await query.CountAsync(ct);

        var products = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var mapped = mapper.Map<List<ProductResponse>>(products);

        logger.LogInformation("Found {Count} products on page {PageNumber}", mapped.Count, pageNumber);

        return Result.Success(new PaginatedList<ProductResponse>(mapped, pageNumber, totalCount, pageSize));
    }

    public async Task<Result<Guid>> CreateAsync(string productName, string productDescription, decimal price, string currency, Guid categoryId, Guid subCategoryId, Guid vendorId, CancellationToken ct = default)
    {
        logger.LogInformation("Creating product: {ProductName}", productName);

        var categoryExists = await context.Categories.AnyAsync(c => c.Id == categoryId, ct);
        if (!categoryExists)
            return Result.Failure<Guid>(CategoryErrors.CategoryNotFound);

        var subCategoryExists = await context.SubCategories.AnyAsync(s => s.Id == subCategoryId, ct);
        if (!subCategoryExists)
            return Result.Failure<Guid>(SubCategoryErrors.SubCategoryNotFound);

        var vendorExists = await context.Vendors.AnyAsync(v => v.Id == vendorId, ct);
        if (!vendorExists)
            return Result.Failure<Guid>(VendorErrors.VendorNotFound);

        var product = new Product
        {
            ProductName = productName,
            ProductDescription = productDescription,
            Price = price,
            Currency = currency,
            CategoryId = categoryId,
            SubCategoryId = subCategoryId,
            VendorId = vendorId
        };

        await context.Products.AddAsync(product, ct);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Product created: {ProductId} - {ProductName}", product.Id, productName);

        return Result.Success(product.Id);
    }

    public async Task<Result> UpdateAsync(Guid id, string productName, string productDescription, decimal price, string currency, CancellationToken ct = default)
    {
        logger.LogInformation("Updating product: {ProductId}", id);

        var product = await context.Products.FindAsync([id], ct);

        if (product is null)
        {
            logger.LogWarning("Product not found for update: {ProductId}", id);
            return Result.Failure(ProductErrors.ProductNotFound);
        }

        product.ProductName = productName;
        product.ProductDescription = productDescription;
        product.Price = price;
        product.Currency = currency;

        await context.SaveChangesAsync(ct);

        logger.LogInformation("Product updated: {ProductId}", id);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        logger.LogInformation("Deleting product: {ProductId}", id);

        var product = await context.Products.FindAsync([id], ct);

        if (product is null)
        {
            logger.LogWarning("Product not found for delete: {ProductId}", id);
            return Result.Failure(ProductErrors.ProductNotFound);
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Product deleted: {ProductId}", id);

        return Result.Success();
    }
}