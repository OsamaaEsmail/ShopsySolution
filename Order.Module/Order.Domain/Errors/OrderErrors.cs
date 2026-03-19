using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;

namespace Order.Domain.Errors;

public record class OrderErrors
{
    public static readonly Error OrderNotFound =
        new("Order.OrderNotFound", "Order is not found", StatusCodes.Status404NotFound);

    public static readonly Error EmptyOrder =
        new("Order.EmptyOrder", "Order must have at least one item", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidStatus =
        new("Order.InvalidStatus", "Invalid order status transition", StatusCodes.Status400BadRequest);

    public static readonly Error OrderAlreadyCancelled =
        new("Order.AlreadyCancelled", "Order is already cancelled", StatusCodes.Status400BadRequest);

    public static readonly Error OrderAlreadyDelivered =
        new("Order.AlreadyDelivered", "Cannot modify a delivered order", StatusCodes.Status400BadRequest);
}