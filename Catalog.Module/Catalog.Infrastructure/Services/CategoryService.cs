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

public class CategoryService(CatalogDbContext context, IMapper mapper, ILogger<CategoryService> logger) : ICategoryService
{
    public async Task<Result<PaginatedList<CategoryResponse>>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        logger.LogInformation("Getting categories, Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

        var query = context.Categories
            .Include(c => c.SubCategories)
            .AsNoTracking();

        var totalCount = await query.CountAsync(ct);

        var categories = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var mapped = mapper.Map<List<CategoryResponse>>(categories);

        logger.LogInformation("Found {Count} categories on page {PageNumber}", mapped.Count, pageNumber);

        return Result.Success(new PaginatedList<CategoryResponse>(mapped, pageNumber, totalCount, pageSize));
    }
    public async Task<Result<CategoryResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        logger.LogInformation("Getting category by ID: {CategoryId}", id);

        var category = await context.Categories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (category is null)
        {
            logger.LogWarning("Category not found: {CategoryId}", id);
            return Result.Failure<CategoryResponse>(CategoryErrors.CategoryNotFound);
        }

        return Result.Success(mapper.Map<CategoryResponse>(category));
    }

    public async Task<Result<Guid>> CreateAsync(string categoryName, CancellationToken ct = default)
    {
        logger.LogInformation("Creating category: {CategoryName}", categoryName);

        var exists = await context.Categories.AnyAsync(c => c.CategoryName == categoryName, ct);

        if (exists)
        {
            logger.LogWarning("Duplicate category: {CategoryName}", categoryName);
            return Result.Failure<Guid>(CategoryErrors.DuplicatedCategory);
        }

        var category = new Category { CategoryName = categoryName };

        await context.Categories.AddAsync(category, ct);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Category created: {CategoryId} - {CategoryName}", category.Id, categoryName);

        return Result.Success(category.Id);
    }

    public async Task<Result> UpdateAsync(Guid id, string categoryName, CancellationToken ct = default)
    {
        logger.LogInformation("Updating category: {CategoryId}", id);

        var category = await context.Categories.FindAsync([id], ct);

        if (category is null)
        {
            logger.LogWarning("Category not found for update: {CategoryId}", id);
            return Result.Failure(CategoryErrors.CategoryNotFound);
        }

        var duplicated = await context.Categories.AnyAsync(c => c.CategoryName == categoryName && c.Id != id, ct);

        if (duplicated)
        {
            logger.LogWarning("Duplicate category name on update: {CategoryName}", categoryName);
            return Result.Failure(CategoryErrors.DuplicatedCategory);
        }

        category.CategoryName = categoryName;
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Category updated: {CategoryId}", id);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        logger.LogInformation("Deleting category: {CategoryId}", id);

        var category = await context.Categories.FindAsync([id], ct);

        if (category is null)
        {
            logger.LogWarning("Category not found for delete: {CategoryId}", id);
            return Result.Failure(CategoryErrors.CategoryNotFound);
        }

        context.Categories.Remove(category);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Category deleted: {CategoryId}", id);

        return Result.Success();
    }
}