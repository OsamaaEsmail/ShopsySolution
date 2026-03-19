using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.Persistence.Configurations;

public class BillingAddressConfiguration : IEntityTypeConfiguration<BillingAddress>
{
    public void Configure(EntityTypeBuilder<BillingAddress> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("BillingAddressId");
        builder.Property(b => b.FullName).IsRequired().HasMaxLength(200);
        builder.Property(b => b.PhoneNumber).IsRequired().HasMaxLength(50);
        builder.Property(b => b.EmailAddress).IsRequired().HasMaxLength(200);
        builder.Property(b => b.Address).IsRequired().HasMaxLength(500);
        builder.Property(b => b.Country).IsRequired().HasMaxLength(100);
        builder.Property(b => b.StateOrProvince).HasMaxLength(100);
        builder.Property(b => b.City).IsRequired().HasMaxLength(100);
        builder.Property(b => b.PostalOrZipCode).HasMaxLength(20);
    }
}