using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.SharedExtensions;
using User.Application.Users.Commands.ChangePassword;
using User.Application.Users.Commands.UpdateProfile;
using User.Application.Users.Queries.GetUserProfile;


namespace Shopsy.API.Controllers.UserController;


[ApiVersion(1)]
[Route("api/me")]
[ApiController]
[Authorize]
public class AccountController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProfile(CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await mediator.Send(new GetUserProfileQuery(userId), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("info")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await mediator.Send(new UpdateProfileCommand(userId, request.FirstName, request.LastName), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await mediator.Send(new ChangePasswordCommand(userId, request.CurrentPassword, request.NewPassword), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}

public record UpdateProfileRequest(string FirstName, string LastName);
public record ChangePasswordRequest(string CurrentPassword, string NewPassword);