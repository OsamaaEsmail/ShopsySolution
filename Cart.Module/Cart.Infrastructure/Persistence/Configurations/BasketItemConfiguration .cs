using Cart.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cart.Infrastructure.Persistence.Configurations;

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.ToTable("BasketItems");
        builder.HasKey(ci => ci.Id);
        builder.Property(ci => ci.UnitPrice).HasPrecision(18, 2);
        builder.Property(ci => ci.Color).HasMaxLength(50);
        builder.Property(ci => ci.Size).HasMaxLength(50);
        builder.Ignore(ci => ci.TotalPrice);
    }
}