using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopsy.BuildingBlocks.Abstractions;
using User.Application.Users.Commands.CreateUser;
using User.Application.Users.Commands.ToggleUserStatus;
using User.Application.Users.Commands.UpdateUser;
using User.Application.Users.Queries.GetAllUsers;
using User.Application.Users.Queries.GetUserById;
using User.Domain.Consts;
using User.Infrastructure.Authentication.Filters;

namespace Shopsy.API.Controllers.User;

[ApiVersion(1)]
[Route("api/[controller]")]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.GetUsers)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchValue = null,
        [FromQuery] string? sortColumn = null,
        [FromQuery] string? sortDirection = null,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(
            new GetAllUsersQuery(pageNumber, pageSize, searchValue, sortColumn, sortDirection), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.GetUsers)]
    public async Task<IActionResult> GetById(string id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.AddUsers)]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateUserRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(
            new UpdateUserCommand(id, request.FirstName, request.LastName, request.Roles), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("{id}/toggle-status")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> ToggleStatus(string id, CancellationToken ct)
    {
        var result = await mediator.Send(new ToggleUserStatusCommand(id), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}

public record UpdateUserRequest(string FirstName, string LastName, IEnumerable<string> Roles);