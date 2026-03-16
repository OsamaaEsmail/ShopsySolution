







using Shopsy.BuildingBlocks.CQRS;

namespace User.Application.Users.Commands.ConfirmEmail;


public record ConfirmEmailCommand(string UserId, string Code) : ICommand;