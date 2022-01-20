using FluentValidation;
using ServerApi.DTO.Pin;

namespace ServerApi.Validation
{
    public class AddPinValidator : AbstractValidator<AddPinRequest>
    {
        public AddPinValidator()
        {
            RuleFor(x => x.PinName).NotEmpty().MinimumLength(2).MaximumLength(256);
            RuleFor(x => x.Description).MaximumLength(2048);
            RuleFor(x => x.Coordinates).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
