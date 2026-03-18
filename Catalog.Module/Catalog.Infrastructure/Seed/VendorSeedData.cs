using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Seed;

public static class VendorSeedData
{
    public static readonly Guid AppleVendorId = Guid.Parse("11111111-aaaa-bbbb-cccc-111111111111");
    public static readonly Guid NikeVendorId = Guid.Parse("22222222-aaaa-bbbb-cccc-222222222222");
    public static readonly Guid SamsungVendorId = Guid.Parse("33333333-aaaa-bbbb-cccc-333333333333");

    public static async Task SeedAsync(CatalogDbContext ctx)
    {
        if (await ctx.Vendors.AnyAsync()) return;

        var vendors = new List<Vendor>
        {
            new() { Id = AppleVendorId, VendorName = "Apple Store", Email = "apple@store.com", PhoneNumber = "+1234567890" },
            new() { Id = NikeVendorId, VendorName = "Nike Official", Email = "nike@store.com", PhoneNumber = "+0987654321" },
            new() { Id = SamsungVendorId, VendorName = "Samsung Store", Email = "samsung@store.com", PhoneNumber = "+1122334455" }
        };

        await ctx.Vendors.AddRangeAsync(vendors);
        await ctx.SaveChangesAsync();
    }
}