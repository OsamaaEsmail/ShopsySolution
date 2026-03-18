using FluentValidation;

namespace Catalog.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryName).NotEmpty().MaximumLength(100);
    }
}