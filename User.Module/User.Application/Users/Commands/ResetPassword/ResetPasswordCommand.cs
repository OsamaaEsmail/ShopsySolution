




using Shopsy.BuildingBlocks.CQRS;

namespace User.Application.Users.Commands.ResetPassword;

public record ResetPasswordCommand(string Email, string Code, string NewPassword) : ICommand;