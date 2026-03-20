using Cart.Application.DtoContracts;
using Cart.Application.Interfaces;
using Cart.Domain.Entities;
using Cart.Domain.Errors;
using Cart.Infrastructure.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shopsy.BuildingBlocks.Abstractions;

namespace Cart.Infrastructure.Services;

public class CartService(CartDbContext context, IMapper mapper, ILogger<CartService> logger) : ICartService
{
    public async Task<Result<CartResponse>> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        logger.LogInformation("Getting cart for user: {UserId}", userId);

        var basket = await context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.UserId == userId, ct);

        if (basket is null)
        {
            logger.LogWarning("Cart not found for user: {UserId}", userId);
            return Result.Failure<CartResponse>(BasketErrors.CartNotFound);
        }

        logger.LogInformation("Cart found with {Count} items for user: {UserId}", basket.Items.Count, userId);

        return Result.Success(mapper.Map<CartResponse>(basket));
    }

    public async Task<Result<Guid>> AddToCartAsync(string userId, Guid productId, string? color, string? size, decimal unitPrice, int quantity, CancellationToken ct = default)
    {
        logger.LogInformation("Adding to cart for user: {UserId}, Product: {ProductId}, Qty: {Quantity}", userId, productId, quantity);

        var basket = await context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.UserId == userId, ct);

        if (basket is null)
        {
            logger.LogInformation("Creating new cart for user: {UserId}", userId);

            basket = new Basket { UserId = userId };
            basket.Items.Add(new BasketItem
            {
                Id = basket.Id,
                ProductId = productId,
                Color = color,
                Size = size,
                UnitPrice = unitPrice,
                Quantity = quantity
            });
            await context.Baskets.AddAsync(basket, ct);
        }
        else
        {
            var existingItem = basket.Items.FirstOrDefault(i =>
                i.ProductId == productId && i.Color == color && i.Size == size);

            if (existingItem is not null)
            {
                logger.LogInformation("Updating quantity for existing cart item: {CartItemId}", existingItem.Id);
                existingItem.Quantity += quantity;
            }
            else
            {
                logger.LogInformation("Adding new item to existing cart");
                basket.Items.Add(new BasketItem
                {
                    Id = basket.Id,
                    ProductId = productId,
                    Color = color,
                    Size = size,
                    UnitPrice = unitPrice,
                    Quantity = quantity
                });
            }
        }

        await context.SaveChangesAsync(ct);

        logger.LogInformation("Cart updated for user: {UserId}, CartId: {CartId}", userId, basket.Id);

        return Result.Success(basket.Id);
    }

    public async Task<Result> RemoveFromCartAsync(string userId, Guid cartItemId, CancellationToken ct = default)
    {
        logger.LogInformation("Removing item: {CartItemId} from cart for user: {UserId}", cartItemId, userId);

        var basket = await context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.UserId == userId, ct);

        if (basket is null)
        {
            logger.LogWarning("Cart not found for user: {UserId}", userId);
            return Result.Failure(BasketErrors.CartNotFound);
        }

        var item = basket.Items.FirstOrDefault(i => i.Id == cartItemId);

        if (item is null)
        {
            logger.LogWarning("Cart item not found: {CartItemId}", cartItemId);
            return Result.Failure(BasketErrors.CartItemNotFound);
        }

        basket.Items.Remove(item);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Item removed from cart: {CartItemId}", cartItemId);

        return Result.Success();
    }

    public async Task<Result> ClearCartAsync(string userId, CancellationToken ct = default)
    {
        logger.LogInformation("Clearing cart for user: {UserId}", userId);

        var basket = await context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.UserId == userId, ct);

        if (basket is null)
        {
            logger.LogWarning("Cart not found for user: {UserId}", userId);
            return Result.Failure(BasketErrors.CartNotFound);
        }

        if (!basket.Items.Any())
        {
            logger.LogWarning("Cart already empty for user: {UserId}", userId);
            return Result.Failure(BasketErrors.EmptyCart);
        }

        var itemCount = basket.Items.Count;
        context.BasketItems.RemoveRange(basket.Items);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Cart cleared: {Count} items removed for user: {UserId}", itemCount, userId);

        return Result.Success();
    }
}