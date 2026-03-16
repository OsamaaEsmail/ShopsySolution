

using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.DtoContracts;
using User.Application.Interfaces;

namespace User.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUserService userService)
    : IQueryHandler<GetAllUsersQuery, PaginatedList<UserResponse>>
{
    public async Task<Result<PaginatedList<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        return await userService.GetAllAsync(
            request.PageNumber, request.PageSize,
            request.SearchValue, request.SortColumn,
            request.SortDirection, ct);
    }
}
