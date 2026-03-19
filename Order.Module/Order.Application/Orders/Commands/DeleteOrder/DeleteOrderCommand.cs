using Shopsy.BuildingBlocks.CQRS;

namespace Order.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(Guid OrderId) : ICommand;