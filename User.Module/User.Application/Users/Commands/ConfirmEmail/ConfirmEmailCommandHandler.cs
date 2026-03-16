





using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.Interfaces;

namespace User.Application.Users.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler(IAuthService authService)
    : ICommandHandler<ConfirmEmailCommand>
{
    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken ct)
    {
        return await authService.ConfirmEmailAsync(request.UserId, request.Code);
    }
}