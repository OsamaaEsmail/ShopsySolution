



using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Seed;

public static class CategorySeedData
{
    public static readonly Guid ElectronicsCatId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid ClothingCatId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid HomeKitchenCatId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid SportsCatId = Guid.Parse("44444444-4444-4444-4444-444444444444");

    public static async Task SeedAsync(CatalogDbContext ctx)
    {
        if (await ctx.Categories.AnyAsync()) return;

        var categories = new List<Category>
        {
            new() { Id = ElectronicsCatId, CategoryName = "Electronics" },
            new() { Id = ClothingCatId, CategoryName = "Clothing" },
            new() { Id = HomeKitchenCatId, CategoryName = "Home & Kitchen" },
            new() { Id = SportsCatId, CategoryName = "Sports" }
        };

        await ctx.Categories.AddRangeAsync(categories);
        await ctx.SaveChangesAsync();
    }
}