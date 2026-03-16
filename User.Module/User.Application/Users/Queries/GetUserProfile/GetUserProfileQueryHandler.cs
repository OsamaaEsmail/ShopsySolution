



using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.DtoContracts;
using User.Application.Interfaces;

namespace User.Application.Users.Queries.GetUserProfile;

public class GetUserProfileQueryHandler(IUserService userService)
    : IQueryHandler<GetUserProfileQuery, UserProfileResponse>
{
    public async Task<Result<UserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken ct)
    {
        return await userService.GetProfileAsync(request.UserId);
    }
}