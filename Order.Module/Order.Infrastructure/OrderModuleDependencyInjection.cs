using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Interfaces;
using Order.Application.Mapping;
using Order.Application.Orders.Commands.CreateOrder;
using Order.Infrastructure.Persistence;
using Order.Infrastructure.Services;


namespace Order.Infrastructure;

public static class OrderModuleDependencyInjection
{
    public static IServiceCollection AddOrderModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOrderDatabase(configuration)
            .AddOrderServices()
            .AddOrderMapster()
            .AddOrderMediatR();



        return services;
    }

    private static IServiceCollection AddOrderDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    private static IServiceCollection AddOrderServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }

    private static IServiceCollection AddOrderMapster(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(typeof(OrderMappingConfig).Assembly);
        services.AddSingleton(mappingConfig);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
    private static IServiceCollection AddOrderMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));

        return services;
    }

    public static async Task ApplyOrderMigrationsAndSeed(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

        await ctx.Database.MigrateAsync();
    }
}

