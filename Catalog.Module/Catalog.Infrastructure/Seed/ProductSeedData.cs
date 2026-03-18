using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Seed;

public static class ProductSeedData
{
    public static async Task SeedAsync(CatalogDbContext ctx)
    {
        if (await ctx.Products.AnyAsync()) return;

        var products = new List<Product>
        {
            new()
            {
                ProductName = "iPhone 15 Pro",
                ProductDescription = "Latest iPhone with A17 chip",
                Price = 999.99m,
                CategoryId = CategorySeedData.ElectronicsCatId,
                SubCategoryId = SubCategorySeedData.PhonesSubId,
                VendorId = VendorSeedData.AppleVendorId,
                Stocks = new()
                {
                    new() { QuantityAvailable = 50, Color = "Black", Size = "128GB" },
                    new() { QuantityAvailable = 30, Color = "White", Size = "256GB" }
                },
                Images = new()
                {
                    new() { ImageUrl = "https://placehold.co/400x400?text=iPhone15" }
                }
            },
            new()
            {
                ProductName = "MacBook Pro 14",
                ProductDescription = "M3 Pro chip laptop",
                Price = 1999.99m,
                CategoryId = CategorySeedData.ElectronicsCatId,
                SubCategoryId = SubCategorySeedData.LaptopsSubId,
                VendorId = VendorSeedData.AppleVendorId,
                Stocks = new()
                {
                    new() { QuantityAvailable = 20, Color = "Space Gray", Size = "16GB" }
                },
                Images = new()
                {
                    new() { ImageUrl = "https://placehold.co/400x400?text=MacBookPro" }
                }
            },
            new()
            {
                ProductName = "Samsung Galaxy S24",
                ProductDescription = "AI powered smartphone",
                Price = 849.99m,
                CategoryId = CategorySeedData.ElectronicsCatId,
                SubCategoryId = SubCategorySeedData.PhonesSubId,
                VendorId = VendorSeedData.SamsungVendorId,
                Stocks = new()
                {
                    new() { QuantityAvailable = 60, Color = "Violet", Size = "256GB" }
                },
                Images = new()
                {
                    new() { ImageUrl = "https://placehold.co/400x400?text=GalaxyS24" }
                }
            },
            new()
            {
                ProductName = "Nike Air Max 90",
                ProductDescription = "Classic sneakers",
                Price = 129.99m,
                CategoryId = CategorySeedData.ClothingCatId,
                SubCategoryId = SubCategorySeedData.MenSubId,
                VendorId = VendorSeedData.NikeVendorId,
                Stocks = new()
                {
                    new() { QuantityAvailable = 100, Color = "White", Size = "42" },
                    new() { QuantityAvailable = 80, Color = "Black", Size = "43" }
                },
                Images = new()
                {
                    new() { ImageUrl = "https://placehold.co/400x400?text=AirMax90" }
                }
            },
            new()
            {
                ProductName = "Nike Dri-FIT T-Shirt",
                ProductDescription = "Performance training tee",
                Price = 35.00m,
                CategoryId = CategorySeedData.ClothingCatId,
                SubCategoryId = SubCategorySeedData.MenSubId,
                VendorId = VendorSeedData.NikeVendorId,
                Stocks = new()
                {
                    new() { QuantityAvailable = 200, Color = "Blue", Size = "L" }
                }
            }
        };

        await ctx.Products.AddRangeAsync(products);
        await ctx.SaveChangesAsync();
    }
}