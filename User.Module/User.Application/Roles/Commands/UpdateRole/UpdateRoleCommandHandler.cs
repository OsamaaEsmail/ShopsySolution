



using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.Interfaces;

namespace User.Application.Roles.Commands.UpdateRole;

public class UpdateRoleCommandHandler(IRoleService roleService)
    : ICommandHandler<UpdateRoleCommand>
{
    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken ct)
    {
        return await roleService.UpdateAsync(request.RoleId, request.Name, request.Permissions);
    }
}