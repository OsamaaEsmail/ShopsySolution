using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Payments.Commands.CreatePayment;
using Shopsy.BuildingBlocks.Abstractions;
using User.Domain.Consts;
using User.Infrastructure.Authentication.Filters;

namespace Shopsy.API.Controllers.Order;

[ApiVersion(1)]
[Route("api/[controller]")]
[ApiController]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [HasPermission(Permissions.AddOrders)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
