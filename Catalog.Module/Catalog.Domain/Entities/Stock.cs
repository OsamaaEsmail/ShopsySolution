



namespace Catalog.Domain.Entities;

public class Stock
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Size { get; set; }
    public string? Color { get; set; }
    public int QuantityAvailable { get; set; }
    public Guid ProductId { get; set; }
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public Product? Product { get; set; }
}