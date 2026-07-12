using FluentValidation;
using MyBookBackend.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookBackend.Common.Validators
{
    public  class EditProductvalidator:AbstractValidator<EditProductDto>
    {
        public EditProductvalidator()
        {

            RuleFor(X => X.Title)
                .NotEmpty()
                .WithMessage("Title is required");


            RuleFor(x => x.Price)
     .GreaterThan(0)
     .WithMessage("Price must be greater than 0")
     .LessThanOrEqualTo(10000)
     .WithMessage("Price cannot be more than Rs. 10000");

            RuleFor(x => x.StockQuantity)
                .NotNull()
                .WithMessage("StockQuantity cannot be null")
                 .NotEmpty()
                .WithMessage("StockQuantity is required")
                 .GreaterThan(1)
                .WithMessage("StockQuantity cannot be more than 1");

            RuleFor(x => x.CategoryId)
                .NotNull()
                .WithMessage("Cateogy iD cannot ib null");

        }

    }
}
