




using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.Interfaces;

namespace User.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IUserService userService)
    : ICommandHandler<CreateUserCommand>
{
    public async Task<Result> Handle(CreateUserCommand request, CancellationToken ct)
    {
        return await userService.CreateUserAsync(
            request.FirstName, request.LastName,
            request.Email, request.Password, request.Roles);
    }
}
