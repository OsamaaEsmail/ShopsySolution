using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.DtoContracts;
using User.Application.Interfaces;

namespace User.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserService userService)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        return await userService.GetByIdAsync(request.UserId);
    }
}