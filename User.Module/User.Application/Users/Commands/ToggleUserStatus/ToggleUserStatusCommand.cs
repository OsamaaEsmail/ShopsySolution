




using Shopsy.BuildingBlocks.CQRS;

namespace User.Application.Users.Commands.ToggleUserStatus;


public record ToggleUserStatusCommand(string UserId) : ICommand;