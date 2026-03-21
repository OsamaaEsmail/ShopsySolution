using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Entities;

namespace Sales.Infrastructure.Persistence.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("SaleId");
        builder.Property(s => s.SaleName).IsRequired().HasMaxLength(200);
        builder.Property(s => s.DiscountPercentage).HasPrecision(18, 2);
        builder.Property(s => s.SaleImage).HasMaxLength(500);
        builder.Ignore(s => s.IsActive);

        builder.HasMany(s => s.SaleItems)
            .WithOne(i => i.Sale)
            .HasForeignKey(i => i.SaleId);
    }
}