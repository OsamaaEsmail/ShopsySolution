using Microsoft.AspNetCore.Http;
using Order.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using Shopsy.BuildingBlocks.SharedExtensions;

namespace Order.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(IOrderService orderService, IHttpContextAccessor httpContextAccessor) : ICommandHandler<CreateOrderCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var userId = httpContextAccessor.HttpContext?.User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Result.Failure<Guid>(new Error("Order.Unauthorized", "User is not authenticated", StatusCodes.Status401Unauthorized));

        return await orderService.CreateAsync(
            userId,
            request.Items,
            request.BillingAddress,
            request.ShippingAddress,
            ct);
    }
}