

using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.DtoContracts;
using User.Application.Interfaces;

namespace User.Application.Users.Commands.Login;

public class LoginCommandHandler(IAuthService authService)
    : ICommandHandler<LoginCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken ct)
    {
        return await authService.GetTokenAsync(request.Email, request.Password, ct);
    }
}