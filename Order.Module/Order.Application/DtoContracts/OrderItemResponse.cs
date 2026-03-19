namespace Order.Application.DtoContracts;

public record OrderItemResponse(
    Guid OrderItemId,
    Guid ProductId,
    string? Size,
    string? Color,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice);