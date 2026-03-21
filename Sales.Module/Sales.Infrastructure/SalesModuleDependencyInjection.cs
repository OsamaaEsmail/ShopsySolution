using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sales.Application.Interfaces;
using Sales.Application.Mapping;
using Sales.Infrastructure.Persistence;
using Sales.Infrastructure.Services;


namespace Sales.Infrastructure;

public static class SalesModuleDependencyInjection
{
    public static IServiceCollection AddSalesModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSalesDatabase(configuration)
            .AddSalesServices()
            .AddSalesMapster();

        return services;
    }

    private static IServiceCollection AddSalesDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SalesDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    private static IServiceCollection AddSalesServices(this IServiceCollection services)
    {
        services.AddScoped<ISaleService, SaleService>();

        return services;
    }

    private static IServiceCollection AddSalesMapster(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(typeof(SaleMappingConfig).Assembly);
        services.AddSingleton(mappingConfig);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }

    public static async Task ApplySalesMigrationsAndSeed(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SalesDbContext>();

        await ctx.Database.MigrateAsync();
    }
}
