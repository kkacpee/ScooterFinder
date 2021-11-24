using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using ServerApi.Validation;

namespace ServerApi.Extensions
{
    public static class IMvcBuilderExtension
    {
        public static IMvcBuilder AddAndConfigureFluentValidation(this IMvcBuilder builder)
        {
            return builder
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssemblyContaining<ScooterValidator>();
                    options.LocalizationEnabled = false;
                    options.DisableDataAnnotationsValidation = false;
                    options.ImplicitlyValidateChildProperties = true;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = new Dictionary<string, IEnumerable<string>>();

                        foreach (var key in context.ModelState.Keys)
                        {
                            var value = context.ModelState[key];
                            if (value.Errors.Any()) errors.Add(key, value.Errors.Select(x => x.ErrorMessage));
                        }

                        return new BadRequestObjectResult(new
                        {
                            StatusCode = (int)StatusCodes.Status400BadRequest,
                            ValidationErrors = errors
                        });
                    };
                });
        }
    }
}
