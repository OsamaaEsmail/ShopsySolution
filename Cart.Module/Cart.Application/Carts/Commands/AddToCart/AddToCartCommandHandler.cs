using Cart.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using Shopsy.BuildingBlocks.SharedExtensions;

namespace Cart.Application.Carts.Commands.AddToCart;

public class AddToCartCommandHandler(ICartService cartService, IHttpContextAccessor httpContextAccessor) : ICommandHandler<AddToCartCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddToCartCommand request, CancellationToken ct)
    {
        var userId = httpContextAccessor.HttpContext?.User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Result.Failure<Guid>(new Error("Cart.Unauthorized", "User is not authenticated", StatusCodes.Status401Unauthorized));

        return await cartService.AddToCartAsync(
            userId,
            request.ProductId,
            request.Color,
            request.Size,
            request.UnitPrice,
            request.Quantity,
            ct);
    }
}