using FluentValidation;
using MyBookBackend.Common.DTO;

namespace MyBookBackend.Common.Validators.Auth
{
    public class RegisterUserValidator
        : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MinimumLength(3)
                .WithMessage("Name must be at least 3 characters.")
                .MaximumLength(50)
                .WithMessage("Name cannot exceed 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.MobileNo)
                .NotEmpty()
                .WithMessage("Mobile number is required.")
                .Matches(@"^[6-9]\d{9}$")
                .WithMessage("Invalid Indian mobile number.");
        }
    }
}