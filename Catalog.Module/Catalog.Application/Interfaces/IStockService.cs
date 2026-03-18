using Catalog.Application.DtoContracts;
using Shopsy.BuildingBlocks.Abstractions;

namespace Catalog.Application.Interfaces;

public interface IStockService
{
    Task<Result<IEnumerable<StockResponse>>> GetByProductAsync(Guid productId, CancellationToken ct = default);
    Task<Result<Guid>> AddAsync(Guid productId, string? size, string? color, int quantity, CancellationToken ct = default);
    Task<Result> UpdateAsync(Guid id, string? size, string? color, int quantity, CancellationToken ct = default);
}