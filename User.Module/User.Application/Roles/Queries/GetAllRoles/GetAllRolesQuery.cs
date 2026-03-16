





using Shopsy.BuildingBlocks.CQRS;
using User.Application.DtoContracts;

namespace User.Application.Roles.Queries.GetAllRoles;

public record GetAllRolesQuery : IQuery<IEnumerable<RoleResponse>>;