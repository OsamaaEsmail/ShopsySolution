using Sales.Application.DtoContracts;
using Sales.Domain.Enums;
using Shopsy.BuildingBlocks.Abstractions;

namespace Sales.Application.Interfaces;

public interface ISaleService
{
    Task<Result<SaleResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<PaginatedList<SaleResponse>>> GetActiveSalesAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    Task<Result<Guid>> CreateAsync(string saleName, SaleType saleType, decimal discountPercentage, DateTime startDate, DateTime endDate, string? saleImage, List<SaleItemResponse> products, CancellationToken ct = default);
    Task<Result> UpdateAsync(Guid id, string saleName, decimal discountPercentage, DateTime startDate, DateTime endDate, string? saleImage, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}