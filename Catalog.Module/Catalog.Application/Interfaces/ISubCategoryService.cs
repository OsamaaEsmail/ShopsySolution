using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Application.Interfaces;

public interface ISubCategoryService
{
    Task<Result<IEnumerable<SubCategoryResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<IEnumerable<SubCategoryResponse>>> GetByCategoryAsync(Guid categoryId, CancellationToken ct = default);
    Task<Result<Guid>> CreateAsync(string subCategoryName, Guid categoryId, CancellationToken ct = default);
    Task<Result> UpdateAsync(Guid id, string subCategoryName, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}