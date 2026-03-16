




using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.Interfaces;

namespace User.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUserService userService)
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        return await userService.UpdateUserAsync(
            request.UserId, request.FirstName,
            request.LastName, request.Roles);
    }
}