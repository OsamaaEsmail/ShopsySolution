






using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.Interfaces;

namespace User.Application.Users.Commands.ResendConfirmationEmail;

public class ResendConfirmationEmailCommandHandler(IAuthService authService)
    : ICommandHandler<ResendConfirmationEmailCommand>
{
    public async Task<Result> Handle(ResendConfirmationEmailCommand request, CancellationToken ct)
    {
        return await authService.ResendConfirmationEmailAsync(request.Email);
    }
}