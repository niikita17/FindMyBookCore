using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyBookBackend.API.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            // Get all action arguments
            var arguments = context.ActionArguments.Values;

            foreach (var argument in arguments)
            {
                // Skip null values
                if (argument == null)
                    continue;

                // Get runtime type of the argument
                var dtoType = argument.GetType();

                // Build IValidator<dtoType>
                var validatorType =
                    typeof(IValidator<>).MakeGenericType(dtoType);

                // Ask DI container for the validator
                var validator =
                    context.HttpContext.RequestServices.GetService(validatorType);

                // If no validator exists, skip
                if (validator == null)
                    continue;

                // Create ValidationContext<T>
                var validationContext =
                    new ValidationContext<object>(argument);

                // Call ValidateAsync using dynamic dispatch
                ValidationResult validationResult =
       await ((dynamic)validator).ValidateAsync((dynamic)validationContext);
                // If validation failed
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .Select(x => new
                        {
                            Field = x.PropertyName,
                            Error = x.ErrorMessage
                        });

                    context.Result = new BadRequestObjectResult(new
                    {
                        Message = "Validation Failed",
                        Errors = errors
                    });

                    return;
                }
            }

            // Continue to the controller
            await next();
        }
    }
}


