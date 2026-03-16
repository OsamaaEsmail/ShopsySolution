




using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.Interfaces;

namespace User.Application.Users.Commands.ResetPassword;

public class ResetPasswordCommandHandler(IAuthService authService)
    : ICommandHandler<ResetPasswordCommand>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken ct)
    {
        return await authService.ResetPasswordAsync(request.Email, request.Code, request.NewPassword);
    }
}