




using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.Interfaces;

namespace User.Application.Users.Commands.RevokeRefreshToken;

public class RevokeRefreshTokenCommandHandler(IAuthService authService)
    : ICommandHandler<RevokeRefreshTokenCommand>
{
    public async Task<Result> Handle(RevokeRefreshTokenCommand request, CancellationToken ct)
    {
        return await authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, ct);
    }
}
