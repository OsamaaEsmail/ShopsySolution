




using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;

namespace User.Domain.Errors;


public record class RoleErrors
{
    public static readonly Error RoleNotFound =
        new("Role.RoleNotFound", "Role is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedRole =
        new("Role.DuplicatedRole", "Role already exists", StatusCodes.Status409Conflict);
}