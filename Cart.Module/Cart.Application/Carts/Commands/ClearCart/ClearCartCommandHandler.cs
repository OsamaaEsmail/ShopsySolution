using Cart.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using Shopsy.BuildingBlocks.SharedExtensions;

namespace Cart.Application.Carts.Commands.ClearCart;

public class ClearCartCommandHandler(ICartService cartService, IHttpContextAccessor httpContextAccessor) : ICommandHandler<ClearCartCommand>
{
    public async Task<Result> Handle(ClearCartCommand request, CancellationToken ct)
    {
        var userId = httpContextAccessor.HttpContext?.User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Result.Failure(new Error("Cart.Unauthorized", "User is not authenticated", StatusCodes.Status401Unauthorized));

        return await cartService.ClearCartAsync(userId, ct);
    }
}