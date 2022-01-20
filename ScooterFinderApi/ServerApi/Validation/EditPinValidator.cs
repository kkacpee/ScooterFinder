using FluentValidation;
using ServerApi.DTO.Pin;

namespace ServerApi.Validation
{
    public class EditPinValidator : AbstractValidator<EditPinRequest>
    {
        public EditPinValidator()
        {
            RuleFor(x => x.PinId).NotEmpty();
            RuleFor(x => x.PinName).MaximumLength(256);
            RuleFor(x => x.Description).MaximumLength(2048);
        }
    }
}
