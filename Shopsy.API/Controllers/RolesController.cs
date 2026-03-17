using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopsy.BuildingBlocks.Abstractions;
using User.Application.Roles.Commands.CreateRole;
using User.Application.Roles.Commands.UpdateRole;
using User.Application.Roles.Queries.GetAllRoles;
using User.Application.Roles.Queries.GetRoleById;
using User.Domain.Consts;
using User.Infrastructure.Authentication.Filters;

namespace Shopsy.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.GetRoles)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllRolesQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.GetRoles)]
    public async Task<IActionResult> GetById(string id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetRoleByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.AddRoles)]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateRoles)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateRoleRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(
            new UpdateRoleCommand(id, request.Name, request.Permissions), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}

public record UpdateRoleRequest(string Name, IList<string> Permissions);