namespace Order.Application.DtoContracts;

public record PaymentResponse(
    Guid PaymentId,
    decimal Amount,
    DateTime PaymentDate,
    string PaymentMethod,
    string PaymentStatus);