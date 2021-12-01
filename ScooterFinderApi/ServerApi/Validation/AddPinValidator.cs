using FluentValidation;
using ServerApi.Persistance.Models;

namespace ServerApi.Validation
{
    public class AddPinValidator : AbstractValidator<Pin>
    {
        public AddPinValidator()
        {
            RuleFor(x => x.PinName).NotEmpty().MinimumLength(2);
        }
    }
}
