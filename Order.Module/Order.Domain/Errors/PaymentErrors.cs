using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;

namespace Order.Domain.Errors;

public record class PaymentErrors
{
    public static readonly Error PaymentNotFound =
        new("Payment.PaymentNotFound", "Payment is not found", StatusCodes.Status404NotFound);

    public static readonly Error PaymentAlreadyExists =
        new("Payment.AlreadyExists", "Payment already exists for this order", StatusCodes.Status409Conflict);

    public static readonly Error InvalidAmount =
        new("Payment.InvalidAmount", "Payment amount does not match order total", StatusCodes.Status400BadRequest);
}