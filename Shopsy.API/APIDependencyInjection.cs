using Asp.Versioning;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Serilog;
using Shopsy.API.Middleware;
using Shopsy.API.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.RateLimiting;

namespace Shopsy.API;

public static class APIDependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        return builder;
    }

    public static WebApplication UseSerilogMiddleware(this WebApplication app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000}ms";
        });

        return app;
    }
    public static WebApplication UseUserActivityLogging(this WebApplication app)
    {
        app.UseMiddleware<UserActivityLoggingMiddleware>();
        return app;
    }
    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // General: 100 request per minute per user
            options.AddPolicy("fixed", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.User?.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1)
                    }));

            // Auth: 10 login attempts per minute per IP
            options.AddPolicy("auth", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(1)
                    }));
        });

        return services;
    }

    public static WebApplication UseRateLimiting(this WebApplication app)
    {
        app.UseRateLimiter();
        return app;
    }

    public static IServiceCollection AddHealthChecking(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(
                connectionString: configuration.GetConnectionString("DefaultConnection")!,
                name: "SQL Server",
                tags: new[] { "database", "sqlserver" })
            .AddUrlGroup(
                new Uri("https://localhost:7146/swagger/index.html"),
                name: "Swagger UI",
                tags: new[] { "api" });

        return services;
    }

    public static WebApplication UseHealthChecking(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecks("/health/database", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("database"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }

    public static WebApplication UseGlobalExceptionHandling(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        return app;
    }
}