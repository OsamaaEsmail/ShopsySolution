namespace Cart.Application.DtoContracts;

public record CartItemResponse(
    Guid CartItemId,
    Guid ProductId,
    string? Color,
    string? Size,
    decimal UnitPrice,
    int Quantity,
    decimal TotalPrice);