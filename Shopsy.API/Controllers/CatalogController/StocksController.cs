
using Asp.Versioning;
using Catalog.Application.Stocks.Commands.AddStock;
using Catalog.Application.Stocks.Commands.UpdateStock;
using Catalog.Application.Stocks.Queries.GetStocksByProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopsy.BuildingBlocks.Abstractions;

namespace Shopsy.API.Controllers.CatalogController;

[ApiVersion(1)]
[Route("api/[controller]")]
[ApiController]
public class StocksController(IMediator mediator) : ControllerBase
{
    [HttpGet("by-product/{productId:guid}")]
    public async Task<IActionResult> GetByProduct(Guid productId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetStocksByProductQuery(productId), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add([FromBody] AddStockCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStockRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(
            new UpdateStockCommand(id, request.Size, request.Color, request.Quantity), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}

public record UpdateStockRequest(string? Size, string? Color, int Quantity);