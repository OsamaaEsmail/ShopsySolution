



using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.Interfaces;

namespace User.Application.Users.Commands.Register;

public class RegisterCommandHandler(IAuthService authService)
    : ICommandHandler<RegisterCommand>
{
    public async Task<Result> Handle(RegisterCommand request, CancellationToken ct)
    {
        return await authService.RegisterAsync(
            request.FirstName, request.LastName,
            request.Email, request.Password, ct);
    }
}