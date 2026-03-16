


using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.DtoContracts;

namespace User.Application.Users.Queries.GetAllUsers;

public record GetAllUsersQuery(
    int PageNumber = 1, int PageSize = 10,
    string? SearchValue = null,
    string? SortColumn = null,
    string? SortDirection = null) : IQuery<PaginatedList<UserResponse>>;