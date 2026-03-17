


using Shopsy.BuildingBlocks.SharedExtensions;

namespace Catalog.Domain.Entities;


public class Product : AuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ProductName { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public bool IsAvailable { get; set; } = true;
    public Guid CategoryId { get; set; }
    public Guid SubCategoryId { get; set; }
    public Guid VendorId { get; set; }

    public List<ProductImage> Images { get; set; } = new();
    public List<Stock> Stocks { get; set; } = new();
    public Category? Category { get; set; }
    public SubCategory? SubCategory { get; set; }
    public Vendor? Vendor { get; set; }
}