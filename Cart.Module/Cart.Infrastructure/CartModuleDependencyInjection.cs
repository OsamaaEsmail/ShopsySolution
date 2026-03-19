using Cart.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cart.Infrastructure;

public static class CartModuleDependencyInjection
{
    public static IServiceCollection AddCartModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCartDatabase(configuration);

        return services;
    }

    private static IServiceCollection AddCartDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CartDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static async Task ApplyCartMigrationsAndSeed(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<CartDbContext>();

        await ctx.Database.MigrateAsync();
    }
}