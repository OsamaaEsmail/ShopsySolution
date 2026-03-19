using Asp.Versioning;
using Catalog.Application.Vendors.Commands.CreateVendor;
using Catalog.Application.Vendors.Commands.DeleteVendor;
using Catalog.Application.Vendors.Commands.UpdateVendor;
using Catalog.Application.Vendors.Queries.GetAllVendors;
using Catalog.Application.Vendors.Queries.GetVendorById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopsy.BuildingBlocks.Abstractions;
using User.Domain.Consts;
using User.Infrastructure.Authentication.Filters;

namespace Shopsy.API.Controllers.Catalog;

[ApiVersion(1)]
[Route("api/[controller]")]
[ApiController]
public class VendorsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.GetVendors)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllVendorsQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id:guid}")]
    [HasPermission(Permissions.GetVendors)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetVendorByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.AddVendors)]
    public async Task<IActionResult> Create([FromBody] CreateVendorCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value) : result.ToProblem();
    }

    [HttpPut("{id:guid}")]
    [HasPermission(Permissions.UpdateVendors)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVendorRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(
            new UpdateVendorCommand(id, request.VendorName, request.Email, request.PhoneNumber, request.VendorPicUrl), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.DeleteVendors)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteVendorCommand(id), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}

public record UpdateVendorRequest(string VendorName, string Email, string PhoneNumber, string? VendorPicUrl);