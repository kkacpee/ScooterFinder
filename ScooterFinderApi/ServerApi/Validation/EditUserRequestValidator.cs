using FluentValidation;
using ServerApi.DTO.User;

namespace ServerApi.Validation
{
    public class EditUserRequestValidator : AbstractValidator<EditUserRequest>
    {
        public EditUserRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.DisplayName).NotEmpty().MinimumLength(3).MaximumLength(256);
        }
    }
}
