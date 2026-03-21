using Cart.Infrastructure.Persistence;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Persistence;
using Sales.Infrastructure.Persistence;

namespace Shopsy.Tests.Helpers;

public static class TestDbContextFactory
{
    public static CatalogDbContext CreateCatalogContext()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var httpContextAccessor = new Microsoft.AspNetCore.Http.HttpContextAccessor();

        return new CatalogDbContext(options, httpContextAccessor);
    }

    public static CartDbContext CreateCartContext()
    {
        var options = new DbContextOptionsBuilder<CartDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new CartDbContext(options);
    }

    public static OrderDbContext CreateOrderContext()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new OrderDbContext(options);
    }

    public static SalesDbContext CreateSalesContext()
    {
        var options = new DbContextOptionsBuilder<SalesDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new SalesDbContext(options);
    }
}