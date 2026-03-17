


namespace Catalog.Domain.Entities;


public class SubCategory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string SubCategoryName { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}