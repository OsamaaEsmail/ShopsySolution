




using Shopsy.BuildingBlocks.CQRS;

namespace User.Application.Roles.Commands.UpdateRole;

public record UpdateRoleCommand(string RoleId, string Name, IList<string> Permissions) : ICommand;