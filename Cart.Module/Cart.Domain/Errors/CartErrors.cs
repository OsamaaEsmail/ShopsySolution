using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;

namespace Cart.Domain.Errors;

public record class CartErrors
{
    public static readonly Error CartNotFound =
        new("Cart.CartNotFound", "Cart is not found", StatusCodes.Status404NotFound);

    public static readonly Error CartItemNotFound =
        new("Cart.CartItemNotFound", "Cart item is not found", StatusCodes.Status404NotFound);

    public static readonly Error EmptyCart =
        new("Cart.EmptyCart", "Cart is already empty", StatusCodes.Status400BadRequest);
}