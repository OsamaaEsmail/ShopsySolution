using FluentValidation;

namespace Sales.Application.Sales.Commands.UpdateSale;

public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleCommandValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty().WithMessage("Sale ID is required");

        RuleFor(x => x.SaleName)
            .NotEmpty().WithMessage("Sale name is required")
            .MaximumLength(200).WithMessage("Sale name must not exceed 200 characters");

        RuleFor(x => x.DiscountPercentage)
            .GreaterThan(0).WithMessage("Discount must be greater than zero");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

        RuleFor(x => x.SaleImage)
            .MaximumLength(500).WithMessage("Sale image URL must not exceed 500 characters")
            .When(x => x.SaleImage is not null);
    }
}