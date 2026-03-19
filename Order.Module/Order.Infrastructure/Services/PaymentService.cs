using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces;
using Order.Domain.Entities;
using Order.Domain.Errors;
using Order.Infrastructure.Persistence;
using Shopsy.BuildingBlocks.Abstractions;

namespace Order.Infrastructure.Services;

public class PaymentService(OrderDbContext context) : IPaymentService
{
    public async Task<Result<Guid>> CreateAsync(Guid orderId, string userId, string paymentMethod, CancellationToken ct = default)
    {
        var order = await context.Orders
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);

        if (order is null)
            return Result.Failure<Guid>(OrderErrors.OrderNotFound);

        if (order.Payment is not null)
            return Result.Failure<Guid>(PaymentErrors.PaymentAlreadyExists);

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

        return Result.Success(payment.Id);
    }
}