



using Shopsy.BuildingBlocks.CQRS;

namespace User.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    string UserId, string FirstName,
    string LastName, IEnumerable<string> Roles) : ICommand;
