using FluentValidation;
using MyBookBackend.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookBackend.Common.Validators
{
    public  class LoginRequestValidator: AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid Message");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("password is required");
      
        }
    }
}
