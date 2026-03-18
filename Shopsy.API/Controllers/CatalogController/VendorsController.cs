using Asp.Versioning;
using Catalog.Application.Vendors.Commands.CreateVendor;
using Catalog.Application.Vendors.Commands.DeleteVendor;
using Catalog.Application.Vendors.Commands.UpdateVendor;
using Catalog.Application.Vendors.Queries.GetAllVendors;
using Catalog.Application.Vendors.Queries.GetVendorById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopsy.BuildingBlocks.Abstractions;

namespace Shopsy.API.Controllers.CatalogController;

[ApiVersion(1)]
[Route("api/[controller]")]
[ApiController]
public class VendorsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllVendorsQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetVendorByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateVendorCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value) : result.ToProblem();
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVendorRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(
            new UpdateVendorCommand(id, request.VendorName, request.Email, request.PhoneNumber, request.VendorPicUrl), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteVendorCommand(id), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}

public record UpdateVendorRequest(string VendorName, string Email, string PhoneNumber, string? VendorPicUrl);