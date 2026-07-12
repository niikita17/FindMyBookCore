using FluentValidation;
using MyBookBackend.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookBackend.Common.Validators
{
    public  class AddToCartValidator:AbstractValidator<AddToCartDto>
    {
        public AddToCartValidator() 
        {

            RuleFor(x => x.BookId)
                .NotEmpty()
                .WithMessage("Book Id is required");
            RuleFor(x => x.Quantity)
                .NotEmpty()
                .WithMessage("Quantity ids required")
                .NotEqual(0)
                .WithMessage("Quantity must be more than 1");
                

        }
    }
}
