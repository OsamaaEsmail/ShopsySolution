using Asp.Versioning;
using Cart.Application.Carts.Commands.AddToCart;
using Cart.Application.Carts.Commands.ClearCart;
using Cart.Application.Carts.Commands.RemoveFromCart;
using Cart.Application.Carts.Queries.GetCartByUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using User.Domain.Consts;
using User.Infrastructure.Authentication.Filters;
using Shopsy.BuildingBlocks.Abstractions;

namespace Shopsy.API.Controllers.Cart;


[ApiVersion(1)]
[Route("api/[controller]")]
[ApiController]
public class CartsController(IMediator mediator) : ControllerBase
{
    [HttpGet("my-cart")]
    [HasPermission(Permissions.GetCarts)]
    public async Task<IActionResult> GetMyCart(CancellationToken ct)
    {
        var result = await mediator.Send(new GetCartByUserQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("add")]
    [HasPermission(Permissions.ManageCarts)]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpDelete("remove/{cartItemId:guid}")]
    [HasPermission(Permissions.ManageCarts)]
    public async Task<IActionResult> RemoveFromCart(Guid cartItemId, CancellationToken ct)
    {
        var result = await mediator.Send(new RemoveFromCartCommand(cartItemId), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("clear")]
    [HasPermission(Permissions.ManageCarts)]
    public async Task<IActionResult> ClearCart(CancellationToken ct)
    {
        var result = await mediator.Send(new ClearCartCommand(), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}