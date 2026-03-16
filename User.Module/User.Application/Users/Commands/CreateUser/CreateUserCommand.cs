




using Shopsy.BuildingBlocks.CQRS;

namespace User.Application.Users.Commands.CreateUser;


public record CreateUserCommand(
    string FirstName, string LastName,
    string Email, string Password,
    IEnumerable<string> Roles) : ICommand;