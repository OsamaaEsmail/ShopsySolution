using Cart.Application.DtoContracts;
using Shopsy.BuildingBlocks.Abstractions;

namespace Cart.Application.Interfaces;

public interface ICartService
{
    Task<Result<CartResponse>> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<Result<Guid>> AddToCartAsync(string userId, Guid productId, string? color, string? size, decimal unitPrice, int quantity, CancellationToken ct = default);
    Task<Result> RemoveFromCartAsync(string userId, Guid cartItemId, CancellationToken ct = default);
    Task<Result> ClearCartAsync(string userId, CancellationToken ct = default);
}