using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sales.Application.DtoContracts;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;
using Sales.Domain.Enums;
using Sales.Domain.Errors;
using Sales.Infrastructure.Persistence;
using Shopsy.BuildingBlocks.Abstractions;

namespace Sales.Infrastructure.Services;

public class SaleService(SalesDbContext context, IMapper mapper, ILogger<SaleService> logger) : ISaleService
{
    public async Task<Result<SaleResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        logger.LogInformation("Getting sale by ID: {SaleId}", id);

        var sale = await context.Sales
            .Include(s => s.SaleItems)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (sale is null)
        {
            logger.LogWarning("Sale not found: {SaleId}", id);
            return Result.Failure<SaleResponse>(SaleErrors.SaleNotFound);
        }

        return Result.Success(mapper.Map<SaleResponse>(sale));
    }

    public async Task<Result<PaginatedList<SaleResponse>>> GetActiveSalesAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Getting active sales, Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

        var now = DateTime.UtcNow;

        var query = context.Sales
            .Include(s => s.SaleItems)
            .Where(s => s.StartDate <= now && s.EndDate >= now)
            .AsNoTracking();

        var totalCount = await query.CountAsync(ct);

        var sales = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var mapped = mapper.Map<List<SaleResponse>>(sales);

        logger.LogInformation("Found {Count} active sales on page {PageNumber}", mapped.Count, pageNumber);

        return Result.Success(new PaginatedList<SaleResponse>(mapped, pageNumber, totalCount, pageSize));
    }

    public async Task<Result<Guid>> CreateAsync(string saleName, SaleType saleType, decimal discountPercentage, DateTime startDate, DateTime endDate, string? saleImage, List<SaleItemResponse> products, CancellationToken ct = default)
    {
        logger.LogInformation("Creating sale: {SaleName}", saleName);

        if (endDate <= startDate)
            return Result.Failure<Guid>(SaleErrors.InvalidDateRange);

        var duplicated = await context.Sales.AnyAsync(s => s.SaleName == saleName, ct);

        if (duplicated)
        {
            logger.LogWarning("Duplicate sale name: {SaleName}", saleName);
            return Result.Failure<Guid>(SaleErrors.DuplicatedSale);
        }

        var sale = new Sale
        {
            SaleName = saleName,
            SaleType = saleType,
            DiscountPercentage = discountPercentage,
            StartDate = startDate,
            EndDate = endDate,
            SaleImage = saleImage
        };

        foreach (var product in products)
        {
            sale.SaleItems.Add(new SaleItem
            {
                SaleId = sale.Id,
                ProductId = product.ProductId,
                DiscountedPrice = product.DiscountedPrice
            });
        }

        await context.Sales.AddAsync(sale, ct);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Sale created: {SaleId} - {SaleName}", sale.Id, saleName);

        return Result.Success(sale.Id);
    }

    public async Task<Result> UpdateAsync(Guid id, string saleName, decimal discountPercentage, DateTime startDate, DateTime endDate, string? saleImage, CancellationToken ct = default)
    {
        logger.LogInformation("Updating sale: {SaleId}", id);

        if (endDate <= startDate)
            return Result.Failure(SaleErrors.InvalidDateRange);

        var sale = await context.Sales.FindAsync([id], ct);

        if (sale is null)
        {
            logger.LogWarning("Sale not found for update: {SaleId}", id);
            return Result.Failure(SaleErrors.SaleNotFound);
        }

        var duplicated = await context.Sales.AnyAsync(s => s.SaleName == saleName && s.Id != id, ct);

        if (duplicated)
        {
            logger.LogWarning("Duplicate sale name on update: {SaleName}", saleName);
            return Result.Failure(SaleErrors.DuplicatedSale);
        }

        sale.SaleName = saleName;
        sale.DiscountPercentage = discountPercentage;
        sale.StartDate = startDate;
        sale.EndDate = endDate;
        sale.SaleImage = saleImage;

        await context.SaveChangesAsync(ct);

        logger.LogInformation("Sale updated: {SaleId}", id);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        logger.LogInformation("Deleting sale: {SaleId}", id);

        var sale = await context.Sales
            .Include(s => s.SaleItems)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (sale is null)
        {
            logger.LogWarning("Sale not found for delete: {SaleId}", id);
            return Result.Failure(SaleErrors.SaleNotFound);
        }

        context.Sales.Remove(sale);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Sale deleted: {SaleId}", id);

        return Result.Success();
    }
}