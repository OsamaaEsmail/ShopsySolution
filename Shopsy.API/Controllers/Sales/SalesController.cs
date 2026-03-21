using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Sales.Application.Sales.Commands.CreateSale;
using Sales.Application.Sales.Commands.DeleteSale;
using Sales.Application.Sales.Commands.UpdateSale;
using Sales.Application.Sales.Queries.GetActiveSales;
using Sales.Application.Sales.Queries.GetSaleById;
using Shopsy.BuildingBlocks.Abstractions;
using User.Domain.Consts;
using User.Infrastructure.Authentication.Filters;


namespace Shopsy.API.Controllers.Sales;

[ApiVersion(1)]
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("fixed")]
public class SalesController(IMediator mediator) : ControllerBase
{
    [HttpGet("active")]
    [HasPermission(Permissions.GetSales)]
    public async Task<IActionResult> GetActiveSales([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetActiveSalesQuery(pageNumber, pageSize), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id:guid}")]
    [HasPermission(Permissions.GetSales)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSaleByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.AddSales)]
    public async Task<IActionResult> Create([FromBody] CreateSaleCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value) : result.ToProblem();
    }

    [HttpPut("{id:guid}")]
    [HasPermission(Permissions.UpdateSales)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(
            new UpdateSaleCommand(id, request.SaleName, request.DiscountPercentage, request.StartDate, request.EndDate, request.SaleImage), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.DeleteSales)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteSaleCommand(id), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}

public record UpdateSaleRequest(string SaleName, decimal DiscountPercentage, DateTime StartDate, DateTime EndDate, string? SaleImage);
