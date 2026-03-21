using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Application.Interfaces;

public interface IProductService
{
    Task<Result<ProductResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<PaginatedList<ProductResponse>>> GetAllAsync(int pageNumber, int pageSize, Guid? categoryId = null, CancellationToken ct = default);
    Task<Result<Guid>> CreateAsync(string productName, string productDescription, decimal price, string currency, Guid categoryId, Guid subCategoryId, Guid vendorId, CancellationToken ct = default);
    Task<Result> UpdateAsync(Guid id, string productName, string productDescription, decimal price, string currency, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}