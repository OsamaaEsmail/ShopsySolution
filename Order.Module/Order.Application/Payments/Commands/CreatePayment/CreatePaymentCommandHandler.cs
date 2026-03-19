using Microsoft.AspNetCore.Http;
using Order.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;
using Shopsy.BuildingBlocks.SharedExtensions;

namespace Order.Application.Payments.Commands.CreatePayment;

public class CreatePaymentCommandHandler(IPaymentService paymentService, IHttpContextAccessor httpContextAccessor) : ICommandHandler<CreatePaymentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreatePaymentCommand request, CancellationToken ct)
    {
        var userId = httpContextAccessor.HttpContext?.User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Result.Failure<Guid>(new Error("Payment.Unauthorized", "User is not authenticated", StatusCodes.Status401Unauthorized));

        return await paymentService.CreateAsync(request.OrderId, userId, request.PaymentMethod, ct);
    }
}