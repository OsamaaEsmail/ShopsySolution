using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Errors;
using Catalog.Infrastructure.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Infrastructure.Services;

public class SubCategoryService(CatalogDbContext context, IMapper mapper) : ISubCategoryService
{
    public async Task<Result<IEnumerable<SubCategoryResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var subCategories = await context.SubCategories
            .AsNoTracking()
            .ToListAsync(ct);

        return Result.Success(mapper.Map<IEnumerable<SubCategoryResponse>>(subCategories));
    }

    public async Task<Result<IEnumerable<SubCategoryResponse>>> GetByCategoryAsync(Guid categoryId, CancellationToken ct = default)
    {
        var categoryExists = await context.Categories.AnyAsync(c => c.Id == categoryId, ct);

        if (!categoryExists)
            return Result.Failure<IEnumerable<SubCategoryResponse>>(CategoryErrors.CategoryNotFound);

        var subCategories = await context.SubCategories
            .Where(s => s.CategoryId == categoryId)
            .AsNoTracking()
            .ToListAsync(ct);

        return Result.Success(mapper.Map<IEnumerable<SubCategoryResponse>>(subCategories));
    }

    public async Task<Result<Guid>> CreateAsync(string subCategoryName, Guid categoryId, CancellationToken ct = default)
    {
        var categoryExists = await context.Categories.AnyAsync(c => c.Id == categoryId, ct);

        if (!categoryExists)
            return Result.Failure<Guid>(CategoryErrors.CategoryNotFound);

        var duplicated = await context.SubCategories.AnyAsync(s => s.SubCategoryName == subCategoryName && s.CategoryId == categoryId, ct);

        if (duplicated)
            return Result.Failure<Guid>(SubCategoryErrors.DuplicatedSubCategory);

        var subCategory = new SubCategory
        {
            SubCategoryName = subCategoryName,
            CategoryId = categoryId
        };

        await context.SubCategories.AddAsync(subCategory, ct);
        await context.SaveChangesAsync(ct);

        return Result.Success(subCategory.Id);
    }

    public async Task<Result> UpdateAsync(Guid id, string subCategoryName, CancellationToken ct = default)
    {
        var subCategory = await context.SubCategories.FindAsync([id], ct);

        if (subCategory is null)
            return Result.Failure(SubCategoryErrors.SubCategoryNotFound);

        subCategory.SubCategoryName = subCategoryName;
        await context.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var subCategory = await context.SubCategories.FindAsync([id], ct);

        if (subCategory is null)
            return Result.Failure(SubCategoryErrors.SubCategoryNotFound);

        context.SubCategories.Remove(subCategory);
        await context.SaveChangesAsync(ct);

        return Result.Success();
    }
}