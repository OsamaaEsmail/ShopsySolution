using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public class Sale
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string SaleName { get; set; } = string.Empty;
    public SaleType SaleType { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? SaleImage { get; set; }
    public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
    public List<SaleItem> SaleItems { get; set; } = new();
}