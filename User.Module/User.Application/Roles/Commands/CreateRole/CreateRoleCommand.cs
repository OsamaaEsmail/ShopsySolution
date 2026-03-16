

using Shopsy.BuildingBlocks.CQRS;

namespace User.Application.Roles.Commands.CreateRole;

public record CreateRoleCommand(string Name, IList<string> Permissions) : ICommand;