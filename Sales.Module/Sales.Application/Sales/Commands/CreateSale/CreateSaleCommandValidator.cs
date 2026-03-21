using FluentValidation;

namespace Sales.Application.Sales.Commands.CreateSale;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.SaleName)
            .NotEmpty().WithMessage("Sale name is required")
            .MaximumLength(200).WithMessage("Sale name must not exceed 200 characters");

        RuleFor(x => x.SaleType)
            .IsInEnum().WithMessage("Invalid sale type");

        RuleFor(x => x.DiscountPercentage)
            .GreaterThan(0).WithMessage("Discount must be greater than zero");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("Sale must have at least one product");

        RuleForEach(x => x.Products).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
                .NotEmpty().WithMessage("Product is required");

            item.RuleFor(i => i.DiscountedPrice)
                .GreaterThan(0).WithMessage("Discounted price must be greater than zero");
        });

        RuleFor(x => x.SaleImage)
            .MaximumLength(500).WithMessage("Sale image URL must not exceed 500 characters")
            .When(x => x.SaleImage is not null);
    }
}