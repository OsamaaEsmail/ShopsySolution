using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.VendorName).IsRequired().HasMaxLength(200);
        builder.Property(v => v.Email).IsRequired().HasMaxLength(200);
        builder.Property(v => v.PhoneNumber).HasMaxLength(50);
        builder.Property(v => v.VendorPicUrl).HasMaxLength(500);
    }
}