using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.Interfaces;

namespace User.Application.Users.Commands.ForgetPassword;

public class ForgetPasswordCommandHandler(IAuthService authService)
    : ICommandHandler<ForgetPasswordCommand>
{
    public async Task<Result> Handle(ForgetPasswordCommand request, CancellationToken ct)
    {
        return await authService.SendResetPasswordCodeAsync(request.Email);
    }
}