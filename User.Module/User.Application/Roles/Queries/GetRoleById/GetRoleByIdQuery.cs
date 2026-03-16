




using Shopsy.BuildingBlocks.CQRS;
using User.Application.DtoContracts;

namespace User.Application.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(string RoleId) : IQuery<RoleDetailResponse>;