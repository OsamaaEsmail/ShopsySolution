using Shopsy.BuildingBlocks.Abstractions;

namespace Order.Application.Interfaces;

public interface IPaymentService
{
    Task<Result<Guid>> CreateAsync(Guid orderId, string userId, string paymentMethod, CancellationToken ct = default);
}