

using Shopsy.BuildingBlocks.CQRS;

namespace User.Application.Users.Commands.Register;

public record RegisterCommand(
    string FirstName, string LastName,
    string Email, string Password) : ICommand;