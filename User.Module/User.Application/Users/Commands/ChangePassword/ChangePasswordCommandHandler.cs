




using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.Interfaces;

namespace User.Application.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler(IUserService userService)
    : ICommandHandler<ChangePasswordCommand>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        return await userService.ChangePasswordAsync(
            request.UserId, request.CurrentPassword, request.NewPassword);
    }
}