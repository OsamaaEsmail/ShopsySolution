using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.Persistence.Configurations;

public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
{
    public void Configure(EntityTypeBuilder<ShippingAddress> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("ShippingAddressId");
        builder.Property(s => s.FullName).IsRequired().HasMaxLength(200);
        builder.Property(s => s.PhoneNumber).IsRequired().HasMaxLength(50);
        builder.Property(s => s.EmailAddress).IsRequired().HasMaxLength(200);
        builder.Property(s => s.Address).IsRequired().HasMaxLength(500);
        builder.Property(s => s.Country).IsRequired().HasMaxLength(100);
        builder.Property(s => s.StateOrProvince).HasMaxLength(100);
        builder.Property(s => s.City).IsRequired().HasMaxLength(100);
        builder.Property(s => s.PostalOrZipCode).HasMaxLength(20);
    }
}