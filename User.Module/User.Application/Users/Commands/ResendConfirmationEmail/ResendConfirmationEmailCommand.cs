





using Shopsy.BuildingBlocks.CQRS;

namespace User.Application.Users.Commands.ResendConfirmationEmail;

public record ResendConfirmationEmailCommand(string Email) : ICommand;