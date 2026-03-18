using FluentValidation;

namespace Catalog.Application.Stocks.Commands.AddStock;

public class AddStockCommandValidator : AbstractValidator<AddStockCommand>
{
    public AddStockCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero");

        RuleFor(x => x.Size)
            .MaximumLength(50).WithMessage("Size must not exceed 50 characters")
            .When(x => x.Size is not null);

        RuleFor(x => x.Color)
            .MaximumLength(50).WithMessage("Color must not exceed 50 characters")
            .When(x => x.Color is not null);
    }
}