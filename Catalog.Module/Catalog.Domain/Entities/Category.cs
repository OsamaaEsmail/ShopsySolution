

namespace Catalog.Domain.Entities;



public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CategoryName { get; set; } = string.Empty;
    public List<SubCategory> SubCategories { get; set; } = new();
}