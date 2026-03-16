

using Shopsy.BuildingBlocks.CQRS;
using User.Application.DtoContracts;

namespace User.Application.Users.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<AuthResponse>;