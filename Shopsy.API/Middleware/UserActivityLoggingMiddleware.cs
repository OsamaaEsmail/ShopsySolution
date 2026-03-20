using Serilog.Context;
using System.Security.Claims;

namespace Shopsy.API.Middleware;

public class UserActivityLoggingMiddleware(RequestDelegate next, ILogger<UserActivityLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
        var userName = context.User.FindFirstValue(ClaimTypes.Name) ?? "Anonymous";
        var method = context.Request.Method;
        var path = context.Request.Path;

        using (LogContext.PushProperty("UserId", userId))
        using (LogContext.PushProperty("UserName", userName))
        {
            logger.LogInformation("[{UserName}] {Method} {Path}", userName, method, path);

            await next(context);
        }
    }
}