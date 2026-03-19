using Cart.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using Shopsy.BuildingBlocks.SharedExtensions;

namespace Cart.Application.Carts.Commands.RemoveFromCart;

public class RemoveFromCartCommandHandler(ICartService cartService, IHttpContextAccessor httpContextAccessor) : ICommandHandler<RemoveFromCartCommand>
{
    public async Task<Result> Handle(RemoveFromCartCommand request, CancellationToken ct)
    {
        var userId = httpContextAccessor.HttpContext?.User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Result.Failure(new Error("Cart.Unauthorized", "User is not authenticated", StatusCodes.Status401Unauthorized));

        return await cartService.RemoveFromCartAsync(userId, request.CartItemId, ct);
    }
}