using Microsoft.AspNetCore.Authorization;




namespace User.Infrastructure.Authentication.Filters;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
