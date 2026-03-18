using Catalog.Application.Interfaces;
using Catalog.Application.Mapping;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Seed;
using Catalog.Infrastructure.Services;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Catalog.Infrastructure;

public static class CatalogModuleDependencyInjection
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddCatalogDatabase(configuration)
            .AddCatalogServices()
            .AddCatalogMapster();

        return services;
    }

    private static IServiceCollection AddCatalogDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    private static IServiceCollection AddCatalogServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISubCategoryService, SubCategoryService>();
        services.AddScoped<IVendorService, VendorService>();
        services.AddScoped<IStockService, StockService>();

        return services;
    }

    private static IServiceCollection AddCatalogMapster(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(typeof(CatalogMappingConfig).Assembly);
        services.AddSingleton(mappingConfig);
        services.AddScoped<IMapper, ServiceMapper>();

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
