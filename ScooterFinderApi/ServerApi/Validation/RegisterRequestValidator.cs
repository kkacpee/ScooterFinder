using FluentValidation;
using ServerApi.DTO;

namespace ServerApi.Validation
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.DisplayName).NotEmpty().MinimumLength(3).MaximumLength(256);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(256);
        }
    }
}
