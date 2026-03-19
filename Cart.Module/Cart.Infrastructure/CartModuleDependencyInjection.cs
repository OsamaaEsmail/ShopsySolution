using Cart.Application.Carts.Commands.AddToCart;
using Cart.Application.Interfaces;
using Cart.Application.Mapping;
using Cart.Infrastructure.Persistence;
using Cart.Infrastructure.Services;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cart.Infrastructure;

public static class CartModuleDependencyInjection
{
    public static IServiceCollection AddCartModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddCartDatabase(configuration)
            .AddCartServices()
            .AddCartMapster()
            .AddCartMediatR();

        return services;
    }

    private static IServiceCollection AddCartDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CartDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    private static IServiceCollection AddCartServices(this IServiceCollection services)
    {
        services.AddScoped<ICartService, CartService>();

        return services;
    }

    private static IServiceCollection AddCartMapster(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(typeof(CartMappingConfig).Assembly);
        services.AddSingleton(mappingConfig);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }

    private static IServiceCollection AddCartMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AddToCartCommand).Assembly));

        return services;
    }

    public static async Task ApplyCartMigrationsAndSeed(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<CartDbContext>();

        await ctx.Database.MigrateAsync();
    }
}