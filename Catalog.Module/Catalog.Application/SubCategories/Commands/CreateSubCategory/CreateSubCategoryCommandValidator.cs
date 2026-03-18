using FluentValidation;

namespace Catalog.Application.SubCategories.Commands.CreateSubCategory;

public class CreateSubCategoryCommandValidator : AbstractValidator<CreateSubCategoryCommand>
{
    public CreateSubCategoryCommandValidator()
    {
        RuleFor(x => x.SubCategoryName)
            .NotEmpty().WithMessage("SubCategory name is required")
            .MaximumLength(100).WithMessage("SubCategory name must not exceed 100 characters");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required");
    }
}