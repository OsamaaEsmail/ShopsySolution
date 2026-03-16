



using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.DtoContracts;
using User.Application.Interfaces;

namespace User.Application.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryHandler(IRoleService roleService)
    : IQueryHandler<GetRoleByIdQuery, RoleDetailResponse>
{
    public async Task<Result<RoleDetailResponse>> Handle(GetRoleByIdQuery request, CancellationToken ct)
    {
        return await roleService.GetByIdAsync(request.RoleId);
    }
}