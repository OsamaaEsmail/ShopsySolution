




using Shopsy.BuildingBlocks.CQRS;

namespace User.Application.Users.Commands.UpdateProfile;


public record UpdateProfileCommand(
    string UserId, string FirstName, string LastName) : ICommand;