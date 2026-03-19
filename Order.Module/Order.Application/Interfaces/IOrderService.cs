using Order.Application.DtoContracts;
using Shopsy.BuildingBlocks.Abstractions;

namespace Order.Application.Interfaces;

public interface IOrderService
{
    Task<Result<OrderResponse>> GetByIdAsync(Guid orderId, CancellationToken ct = default);
    Task<Result<IEnumerable<OrderResponse>>> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<Result<Guid>> CreateAsync(string userId, List<OrderItemResponse> items, AddressResponse billingAddress, AddressResponse shippingAddress, CancellationToken ct = default);
    Task<Result> UpdateStatusAsync(Guid orderId, string status, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid orderId, CancellationToken ct = default);
}