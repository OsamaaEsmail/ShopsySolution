




using FluentValidation;
using User.Domain.Consts;


namespace User.Application.Users.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password must be at least 8 characters with uppercase, lowercase, digit and special character.");
        RuleFor(x => x.FirstName).NotEmpty().Length(4, 50);
        RuleFor(x => x.LastName).NotEmpty().Length(4, 50);
    }
}
