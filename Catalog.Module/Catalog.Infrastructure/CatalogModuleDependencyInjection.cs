using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure;

public static class CatalogModuleDependencyInjection
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCatalogDatabase(configuration);

        return services;
    }

    private static IServiceCollection AddCatalogDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static async Task ApplyCatalogMigrationsAndSeed(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        await ctx.Database.MigrateAsync();

        await CategorySeedData.SeedAsync(ctx);
        await SubCategorySeedData.SeedAsync(ctx);
        await VendorSeedData.SeedAsync(ctx);
        await ProductSeedData.SeedAsync(ctx);
    }
}