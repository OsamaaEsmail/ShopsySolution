namespace Order.Application.DtoContracts;

public record OrderResponse(
    Guid OrderId,
    string UserId,
    DateTime OrderDate,
    string OrderStatus,
    string ShippingStatus,
    int TotalQuantity,
    decimal TotalAmount,
    List<OrderItemResponse> Items,
    AddressResponse? BillingAddress,
    AddressResponse? ShippingAddress,
    PaymentResponse? Payment);