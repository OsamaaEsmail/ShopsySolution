



using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.DtoContracts;
using User.Application.Interfaces;

namespace User.Application.Roles.Queries.GetAllRoles;

public class GetAllRolesQueryHandler(IRoleService roleService)
    : IQueryHandler<GetAllRolesQuery, IEnumerable<RoleResponse>>
{
    public async Task<Result<IEnumerable<RoleResponse>>> Handle(GetAllRolesQuery request, CancellationToken ct)
    {
        return await roleService.GetAllAsync();
    }
}