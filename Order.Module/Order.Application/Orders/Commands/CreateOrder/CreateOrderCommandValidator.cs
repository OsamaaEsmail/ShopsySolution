using FluentValidation;

namespace Order.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Order must have at least one item");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
                .NotEmpty().WithMessage("Product is required");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero");

            item.RuleFor(i => i.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero");
        });

        RuleFor(x => x.BillingAddress).NotNull().WithMessage("Billing address is required");
        RuleFor(x => x.BillingAddress.FullName).NotEmpty().WithMessage("Billing full name is required");
        RuleFor(x => x.BillingAddress.PhoneNumber).NotEmpty().WithMessage("Billing phone number is required");
        RuleFor(x => x.BillingAddress.EmailAddress).NotEmpty().EmailAddress().WithMessage("Valid billing email is required");
        RuleFor(x => x.BillingAddress.Address).NotEmpty().WithMessage("Billing address is required");
        RuleFor(x => x.BillingAddress.Country).NotEmpty().WithMessage("Billing country is required");
        RuleFor(x => x.BillingAddress.City).NotEmpty().WithMessage("Billing city is required");

        RuleFor(x => x.ShippingAddress).NotNull().WithMessage("Shipping address is required");
        RuleFor(x => x.ShippingAddress.FullName).NotEmpty().WithMessage("Shipping full name is required");
        RuleFor(x => x.ShippingAddress.PhoneNumber).NotEmpty().WithMessage("Shipping phone number is required");
        RuleFor(x => x.ShippingAddress.EmailAddress).NotEmpty().EmailAddress().WithMessage("Valid shipping email is required");
        RuleFor(x => x.ShippingAddress.Address).NotEmpty().WithMessage("Shipping address is required");
        RuleFor(x => x.ShippingAddress.Country).NotEmpty().WithMessage("Shipping country is required");
        RuleFor(x => x.ShippingAddress.City).NotEmpty().WithMessage("Shipping city is required");
    }
}