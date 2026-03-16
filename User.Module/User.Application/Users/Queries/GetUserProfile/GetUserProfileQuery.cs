



using Shopsy.BuildingBlocks.CQRS;
using User.Application.DtoContracts;

namespace User.Application.Users.Queries.GetUserProfile;

public record GetUserProfileQuery(string UserId) : IQuery<UserProfileResponse>;