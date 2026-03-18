using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Seed;

public static class SubCategorySeedData
{
    public static readonly Guid PhonesSubId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    public static readonly Guid LaptopsSubId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    public static readonly Guid MenSubId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
    public static readonly Guid WomenSubId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
    public static readonly Guid KitchenAppliancesSubId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
    public static readonly Guid FitnessSubId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");

    public static async Task SeedAsync(CatalogDbContext ctx)
    {
        if (await ctx.SubCategories.AnyAsync()) return;

        var subCategories = new List<SubCategory>
        {
            new() { Id = PhonesSubId, SubCategoryName = "Phones", CategoryId = CategorySeedData.ElectronicsCatId },
            new() { Id = LaptopsSubId, SubCategoryName = "Laptops", CategoryId = CategorySeedData.ElectronicsCatId },
            new() { Id = MenSubId, SubCategoryName = "Men", CategoryId = CategorySeedData.ClothingCatId },
            new() { Id = WomenSubId, SubCategoryName = "Women", CategoryId = CategorySeedData.ClothingCatId },
            new() { Id = KitchenAppliancesSubId, SubCategoryName = "Kitchen Appliances", CategoryId = CategorySeedData.HomeKitchenCatId },
            new() { Id = FitnessSubId, SubCategoryName = "Fitness", CategoryId = CategorySeedData.SportsCatId }
        };

        await ctx.SubCategories.AddRangeAsync(subCategories);
        await ctx.SaveChangesAsync();
    }
}