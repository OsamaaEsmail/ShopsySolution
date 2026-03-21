using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Application.Interfaces;

public interface ICategoryService
{
    Task<Result<PaginatedList<CategoryResponse>>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    Task<Result<CategoryResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<Guid>> CreateAsync(string categoryName, CancellationToken ct = default);
    Task<Result> UpdateAsync(Guid id, string categoryName, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}