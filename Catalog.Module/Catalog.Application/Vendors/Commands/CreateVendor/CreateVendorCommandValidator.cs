using FluentValidation;

namespace Catalog.Application.Vendors.Commands.CreateVendor;

public class CreateVendorCommandValidator : AbstractValidator<CreateVendorCommand>
{
    public CreateVendorCommandValidator()
    {
        RuleFor(x => x.VendorName)
            .NotEmpty().WithMessage("Vendor name is required")
            .MaximumLength(200).WithMessage("Vendor name must not exceed 200 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(200).WithMessage("Email must not exceed 200 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .MaximumLength(50).WithMessage("Phone number must not exceed 50 characters");

        RuleFor(x => x.VendorPicUrl)
            .MaximumLength(500).WithMessage("Vendor picture URL must not exceed 500 characters")
            .When(x => x.VendorPicUrl is not null);
    }
}