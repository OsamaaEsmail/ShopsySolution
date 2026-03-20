using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Application.Interfaces;
using Order.Domain.Entities;
using Order.Domain.Errors;
using Order.Infrastructure.Persistence;
using Shopsy.BuildingBlocks.Abstractions;


namespace Order.Infrastructure.Services;

public class PaymentService(OrderDbContext context, ILogger<PaymentService> logger) : IPaymentService
{
    public async Task<Result<Guid>> CreateAsync(Guid orderId, string userId, string paymentMethod, CancellationToken ct = default)
    {
        logger.LogInformation("Creating payment for order: {OrderId}, Method: {PaymentMethod}", orderId, paymentMethod);

        var order = await context.Orders
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);

        if (order is null)
        {
            logger.LogWarning("Order not found for payment: {OrderId}", orderId);
            return Result.Failure<Guid>(OrderErrors.OrderNotFound);
        }

        if (order.Payment is not null)
        {
            logger.LogWarning("Payment already exists for order: {OrderId}", orderId);
            return Result.Failure<Guid>(PaymentErrors.PaymentAlreadyExists);
        }

        var payment = new Payment
        {
            OrderId = orderId,
            UserId = userId,
            Amount = order.TotalAmount,
            PaymentMethod = paymentMethod,
            PaymentStatus = "Completed"
        };

        await context.Payments.AddAsync(payment, ct);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Payment created: {PaymentId}, Amount: {Amount} for order: {OrderId}", payment.Id, payment.Amount, orderId);

        return Result.Success(payment.Id);
    }
}
