using Cart.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cart.Infrastructure.Persistence.Configurations;

public class BasketConfiguration : IEntityTypeConfiguration<Basket>
{
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        builder.ToTable("Baskets");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("CartId");
        builder.Property(c => c.UserId).IsRequired().HasMaxLength(450);
        builder.Ignore(c => c.TotalQuantity);
        builder.Ignore(c => c.TotalAmount);

        builder.HasMany(c => c.Items)
            .WithOne(i => i.Basket)
            .HasForeignKey(i => i.Id);

        builder.HasIndex(c => c.UserId).IsUnique();
    }
}