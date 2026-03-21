using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Application.Interfaces;

public interface IVendorService
{
    Task<Result<PaginatedList<VendorResponse>>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    Task<Result<VendorResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<Guid>> CreateAsync(string vendorName, string email, string phoneNumber, string? vendorPicUrl, CancellationToken ct = default);
    Task<Result> UpdateAsync(Guid id, string vendorName, string email, string phoneNumber, string? vendorPicUrl, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}