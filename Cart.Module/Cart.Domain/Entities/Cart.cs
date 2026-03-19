namespace Cart.Domain.Entities;

public class Cart
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = string.Empty;
    public int TotalQuantity => Items.Sum(i => i.Quantity);
    public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
    public List<CartItem> Items { get; set; } = new();
}