namespace Cart.Application.DtoContracts;

public record CartResponse(
    Guid CartId,
    string UserId,
    int TotalQuantity,
    decimal TotalAmount,
    List<CartItemResponse> Items);