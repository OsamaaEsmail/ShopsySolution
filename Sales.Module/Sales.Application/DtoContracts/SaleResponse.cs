namespace Sales.Application.DtoContracts;

public record SaleResponse(
    Guid SaleId,
    string SaleName,
    string SaleType,
    decimal DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive,
    string? SaleImage,
    List<SaleItemResponse> Products);