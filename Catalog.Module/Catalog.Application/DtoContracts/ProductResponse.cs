namespace Catalog.Application.DtoContracts;

public record ProductResponse(
    Guid Id,
    string ProductName,
    string ProductDescription,
    decimal Price,
    string Currency,
    bool IsAvailable,
    int TotalStock,
    string CategoryName,
    string SubCategoryName,
    string VendorName,
    List<string> ImageUrls,
    DateTime CreatedOn);