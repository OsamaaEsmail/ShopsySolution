

using Shopsy.BuildingBlocks.CQRS;

namespace User.Application.Users.Commands.ForgetPassword;

public record ForgetPasswordCommand(string Email) : ICommand;