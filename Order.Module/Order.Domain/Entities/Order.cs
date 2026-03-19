using Microsoft.AspNetCore.Http;
using Order.Domain.Enums;

namespace Order.Domain.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public string ShippingStatus { get; set; } = "Pending";
    public int TotalQuantity { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new();
    public BillingAddress? BillingAddress { get; set; }
    public ShippingAddress? ShippingAddress { get; set; }
    public Payment? Payment { get; set; }

    public void RecalculateTotals()
    {
        TotalQuantity = OrderItems.Sum(i => i.Quantity);
        TotalAmount = OrderItems.Sum(i => i.TotalPrice);
    }
}