using Shopsy.BuildingBlocks.CQRS;

namespace Order.Application.Orders.Commands.UpdateOrderStatus;

public record UpdateOrderStatusCommand(Guid OrderId, string Status) : ICommand;