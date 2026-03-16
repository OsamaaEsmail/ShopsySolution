




using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.DtoContracts;
using User.Application.Interfaces;

namespace User.Application.Users.Commands.RefreshToken;

public class RefreshTokenCommandHandler(IAuthService authService)
    : ICommandHandler<RefreshTokenCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        return await authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, ct);
    }
}
