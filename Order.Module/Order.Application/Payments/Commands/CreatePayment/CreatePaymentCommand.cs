using Shopsy.BuildingBlocks.CQRS;

namespace Order.Application.Payments.Commands.CreatePayment;

public record CreatePaymentCommand(Guid OrderId, string PaymentMethod) : ICommand<Guid>;