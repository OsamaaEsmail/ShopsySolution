using FluentValidation;

namespace Order.Application.Payments.Commands.CreatePayment;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order is required");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is required")
            .MaximumLength(50).WithMessage("Payment method must not exceed 50 characters");
    }
}