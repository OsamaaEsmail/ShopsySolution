



using Microsoft.AspNetCore.Authorization;

namespace User.Infrastructure.Authentication.Filters;

public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
{


}
