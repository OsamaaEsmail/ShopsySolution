namespace Sales.Application.DtoContracts;

public record SaleItemResponse(
    Guid SaleItemId,
    Guid ProductId,
    decimal DiscountedPrice);