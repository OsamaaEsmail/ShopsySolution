using System.Security.Claims;

namespace Shopsy.BuildingBlocks.SharedExtensions;

public static class UserExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}