using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Errors;
using Catalog.Infrastructure.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Infrastructure.Services;

public class CategoryService(CatalogDbContext context, IMapper mapper) : ICategoryService
{
    public async Task<Result<IEnumerable<CategoryResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var categories = await context.Categories
            .Include(c => c.SubCategories)
            .AsNoTracking()
            .ToListAsync(ct);

        return Result.Success(mapper.Map<IEnumerable<CategoryResponse>>(categories));
    }

    public async Task<Result<CategoryResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var category = await context.Categories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (category is null)
            return Result.Failure<CategoryResponse>(CategoryErrors.CategoryNotFound);

        return Result.Success(mapper.Map<CategoryResponse>(category));
    }

    public async Task<Result<Guid>> CreateAsync(string categoryName, CancellationToken ct = default)
    {
        var exists = await context.Categories.AnyAsync(c => c.CategoryName == categoryName, ct);

        if (exists)
            return Result.Failure<Guid>(CategoryErrors.DuplicatedCategory);

        var category = new Category { CategoryName = categoryName };

        await context.Categories.AddAsync(category, ct);
        await context.SaveChangesAsync(ct);

        return Result.Success(category.Id);
    }

    public async Task<Result> UpdateAsync(Guid id, string categoryName, CancellationToken ct = default)
    {
        var category = await context.Categories.FindAsync([id], ct);

        if (category is null)
            return Result.Failure(CategoryErrors.CategoryNotFound);

        var duplicated = await context.Categories.AnyAsync(c => c.CategoryName == categoryName && c.Id != id, ct);

        if (duplicated)
            return Result.Failure(CategoryErrors.DuplicatedCategory);

        category.CategoryName = categoryName;
        await context.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var category = await context.Categories.FindAsync([id], ct);

        if (category is null)
            return Result.Failure(CategoryErrors.CategoryNotFound);

        context.Categories.Remove(category);
        await context.SaveChangesAsync(ct);

        return Result.Success();
    }
}