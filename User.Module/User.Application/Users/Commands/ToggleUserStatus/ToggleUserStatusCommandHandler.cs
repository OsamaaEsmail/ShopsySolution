





using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.Interfaces;

namespace User.Application.Users.Commands.ToggleUserStatus;

public class ToggleUserStatusCommandHandler(IUserService userService)
    : ICommandHandler<ToggleUserStatusCommand>
{
    public async Task<Result> Handle(ToggleUserStatusCommand request, CancellationToken ct)
    {
        return await userService.ToggleStatusAsync(request.UserId);
    }
}