using Cart.Application.DtoContracts;
using Cart.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using Shopsy.BuildingBlocks.SharedExtensions;


namespace Cart.Application.Carts.Queries.GetCartByUser;

public class GetCartByUserQueryHandler(ICartService cartService, IHttpContextAccessor httpContextAccessor) : IQueryHandler<GetCartByUserQuery, CartResponse>
{
    public async Task<Result<CartResponse>> Handle(GetCartByUserQuery request, CancellationToken ct)
    {
        var userId = httpContextAccessor.HttpContext?.User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Result.Failure<CartResponse>(new Error("Cart.Unauthorized", "User is not authenticated", StatusCodes.Status401Unauthorized));

        return await cartService.GetByUserIdAsync(userId, ct);
    }
}
