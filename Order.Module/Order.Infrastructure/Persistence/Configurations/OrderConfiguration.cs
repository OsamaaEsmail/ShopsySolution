using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;
using OrderEntity = Order.Domain.Entities.Order;

namespace Order.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasColumnName("OrderId");
        builder.Property(o => o.UserId).IsRequired().HasMaxLength(450);
        builder.Property(o => o.TotalAmount).HasPrecision(18, 2);
        builder.Property(o => o.ShippingStatus).HasMaxLength(50);

        builder.HasMany(o => o.OrderItems)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId);

        builder.HasOne(o => o.BillingAddress)
            .WithOne(b => b.Order)
            .HasForeignKey<BillingAddress>(b => b.OrderId);

        builder.HasOne(o => o.ShippingAddress)
            .WithOne(s => s.Order)
            .HasForeignKey<ShippingAddress>(s => s.OrderId);

        builder.HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<Payment>(p => p.OrderId);
    }
}