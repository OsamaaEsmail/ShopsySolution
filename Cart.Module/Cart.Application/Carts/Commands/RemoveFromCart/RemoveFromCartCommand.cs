using Shopsy.BuildingBlocks.CQRS;

namespace Cart.Application.Carts.Commands.RemoveFromCart;

public record RemoveFromCartCommand(Guid CartItemId) : ICommand;