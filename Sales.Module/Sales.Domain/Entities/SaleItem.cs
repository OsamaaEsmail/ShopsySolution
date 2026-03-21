namespace Sales.Domain.Entities;

public class SaleItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public decimal DiscountedPrice { get; set; }
    public Sale? Sale { get; set; }
}