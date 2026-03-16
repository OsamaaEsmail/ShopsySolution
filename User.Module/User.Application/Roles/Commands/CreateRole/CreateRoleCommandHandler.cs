

using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using User.Application.Interfaces;

namespace User.Application.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler(IRoleService roleService)
    : ICommandHandler<CreateRoleCommand>
{
    public async Task<Result> Handle(CreateRoleCommand request, CancellationToken ct)
    {
        return await roleService.CreateAsync(request.Name, request.Permissions);
    }
}