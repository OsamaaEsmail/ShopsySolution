using Shopsy.BuildingBlocks.CQRS;

namespace Cart.Application.Carts.Commands.AddToCart;

public record AddToCartCommand(
    Guid ProductId,
    string? Color,
    string? Size,
    decimal UnitPrice,
    int Quantity) : ICommand<Guid>;