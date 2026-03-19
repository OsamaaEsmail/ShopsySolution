using Microsoft.AspNetCore.Http;
using Order.Application.DtoContracts;
using Order.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using Shopsy.BuildingBlocks.SharedExtensions;

namespace Order.Application.Orders.Queries.GetMyOrders;

public class GetMyOrdersQueryHandler(IOrderService orderService, IHttpContextAccessor httpContextAccessor) : IQueryHandler<GetMyOrdersQuery, IEnumerable<OrderResponse>>
{
    public async Task<Result<IEnumerable<OrderResponse>>> Handle(GetMyOrdersQuery request, CancellationToken ct)
    {
        var userId = httpContextAccessor.HttpContext?.User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Result.Failure<IEnumerable<OrderResponse>>(new Error("Order.Unauthorized", "User is not authenticated", StatusCodes.Status401Unauthorized));

        return await orderService.GetByUserIdAsync(userId, ct);
    }
}