using Order.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Order.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler(IOrderService orderService) : ICommandHandler<DeleteOrderCommand>
{
    public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken ct)
    {
        return await orderService.DeleteAsync(request.OrderId, ct);
    }
}