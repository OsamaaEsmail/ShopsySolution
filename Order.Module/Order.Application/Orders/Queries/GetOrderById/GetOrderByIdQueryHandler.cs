using Order.Application.DtoContracts;
using Order.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Order.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler(IOrderService orderService) : IQueryHandler<GetOrderByIdQuery, OrderResponse>
{
    public async Task<Result<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        return await orderService.GetByIdAsync(request.OrderId, ct);
    }
}