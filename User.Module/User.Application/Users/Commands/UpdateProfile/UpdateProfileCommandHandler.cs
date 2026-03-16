




using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.Interfaces;

namespace User.Application.Users.Commands.UpdateProfile;

public class UpdateProfileCommandHandler(IUserService userService)
    : ICommandHandler<UpdateProfileCommand>
{
    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken ct)
    {
        return await userService.UpdateProfileAsync(
            request.UserId, request.FirstName, request.LastName);
    }
}