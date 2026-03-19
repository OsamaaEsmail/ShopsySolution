using Cart.Application.DtoContracts;
using Cart.Application.Interfaces;
using Cart.Domain.Entities;
using Cart.Domain.Errors;
using Cart.Infrastructure.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Shopsy.BuildingBlocks.Abstractions;

namespace Cart.Infrastructure.Services;

public class CartService(CartDbContext context, IMapper mapper) : ICartService
{
    public async Task<Result<CartResponse>> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        var basket = await context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.UserId == userId, ct);

        if (basket is null)
            return Result.Failure<CartResponse>(BasketErrors.CartNotFound);

        return Result.Success(mapper.Map<CartResponse>(basket));
    }

    public async Task<Result<Guid>> AddToCartAsync(string userId, Guid productId, string? color, string? size, decimal unitPrice, int quantity, CancellationToken ct = default)
    {
        var basket = await context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.UserId == userId, ct);

        if (basket is null)
        {
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
                existingItem.Quantity += quantity;
            }
            else
            {
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
        return Result.Success(basket.Id);
    }

    public async Task<Result> RemoveFromCartAsync(string userId, Guid cartItemId, CancellationToken ct = default)
    {
        var basket = await context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.UserId == userId, ct);

        if (basket is null)
            return Result.Failure(BasketErrors.CartNotFound);

        var item = basket.Items.FirstOrDefault(i => i.Id == cartItemId);

        if (item is null)
            return Result.Failure(BasketErrors.CartItemNotFound);

        basket.Items.Remove(item);
        await context.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> ClearCartAsync(string userId, CancellationToken ct = default)
    {
        var basket = await context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.UserId == userId, ct);

        if (basket is null)
            return Result.Failure(BasketErrors.CartNotFound);

        if (!basket.Items.Any())
            return Result.Failure(BasketErrors.EmptyCart);

        context.BasketItems.RemoveRange(basket.Items);
        await context.SaveChangesAsync(ct);

        return Result.Success();
    }
}