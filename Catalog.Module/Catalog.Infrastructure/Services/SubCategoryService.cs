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

public class SubCategoryService(CatalogDbContext context, IMapper mapper, ILogger<SubCategoryService> logger) : ISubCategoryService
{
    public async Task<Result<IEnumerable<SubCategoryResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        logger.LogInformation("Getting all subcategories");

        var subCategories = await context.SubCategories
            .AsNoTracking()
            .ToListAsync(ct);

        logger.LogInformation("Found {Count} subcategories", subCategories.Count);

        return Result.Success(mapper.Map<IEnumerable<SubCategoryResponse>>(subCategories));
    }

    public async Task<Result<IEnumerable<SubCategoryResponse>>> GetByCategoryAsync(Guid categoryId, CancellationToken ct = default)
    {
        logger.LogInformation("Getting subcategories by category: {CategoryId}", categoryId);

        var categoryExists = await context.Categories.AnyAsync(c => c.Id == categoryId, ct);

        if (!categoryExists)
        {
            logger.LogWarning("Category not found: {CategoryId}", categoryId);
            return Result.Failure<IEnumerable<SubCategoryResponse>>(CategoryErrors.CategoryNotFound);
        }

        var subCategories = await context.SubCategories
            .Where(s => s.CategoryId == categoryId)
            .AsNoTracking()
            .ToListAsync(ct);

        return Result.Success(mapper.Map<IEnumerable<SubCategoryResponse>>(subCategories));
    }

    public async Task<Result<Guid>> CreateAsync(string subCategoryName, Guid categoryId, CancellationToken ct = default)
    {
        logger.LogInformation("Creating subcategory: {SubCategoryName} in category: {CategoryId}", subCategoryName, categoryId);

        var categoryExists = await context.Categories.AnyAsync(c => c.Id == categoryId, ct);

        if (!categoryExists)
            return Result.Failure<Guid>(CategoryErrors.CategoryNotFound);

        var duplicated = await context.SubCategories.AnyAsync(s => s.SubCategoryName == subCategoryName && s.CategoryId == categoryId, ct);

        if (duplicated)
        {
            logger.LogWarning("Duplicate subcategory: {SubCategoryName}", subCategoryName);
            return Result.Failure<Guid>(SubCategoryErrors.DuplicatedSubCategory);
        }

        var subCategory = new SubCategory
        {
            SubCategoryName = subCategoryName,
            CategoryId = categoryId
        };

        await context.SubCategories.AddAsync(subCategory, ct);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("SubCategory created: {SubCategoryId} - {SubCategoryName}", subCategory.Id, subCategoryName);

        return Result.Success(subCategory.Id);
    }

    public async Task<Result> UpdateAsync(Guid id, string subCategoryName, CancellationToken ct = default)
    {
        logger.LogInformation("Updating subcategory: {SubCategoryId}", id);

        var subCategory = await context.SubCategories.FindAsync([id], ct);

        if (subCategory is null)
        {
            logger.LogWarning("SubCategory not found for update: {SubCategoryId}", id);
            return Result.Failure(SubCategoryErrors.SubCategoryNotFound);
        }

        subCategory.SubCategoryName = subCategoryName;
        await context.SaveChangesAsync(ct);

        logger.LogInformation("SubCategory updated: {SubCategoryId}", id);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        logger.LogInformation("Deleting subcategory: {SubCategoryId}", id);

        var subCategory = await context.SubCategories.FindAsync([id], ct);

        if (subCategory is null)
        {
            logger.LogWarning("SubCategory not found for delete: {SubCategoryId}", id);
            return Result.Failure(SubCategoryErrors.SubCategoryNotFound);
        }

        context.SubCategories.Remove(subCategory);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("SubCategory deleted: {SubCategoryId}", id);

        return Result.Success();
    }
}