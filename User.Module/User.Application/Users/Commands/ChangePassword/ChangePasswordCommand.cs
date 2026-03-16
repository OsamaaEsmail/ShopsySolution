



using Shopsy.BuildingBlocks.CQRS;

namespace User.Application.Users.Commands.ChangePassword;

public record ChangePasswordCommand(
    string UserId, string CurrentPassword, string NewPassword) : ICommand;