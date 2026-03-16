





using Shopsy.BuildingBlocks.CQRS;

namespace User.Application.Users.Commands.RevokeRefreshToken;

public record RevokeRefreshTokenCommand(string Token, string RefreshToken) : ICommand;