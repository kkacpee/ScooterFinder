using FluentValidation;
using ServerApi.Persistance.Models;

namespace ServerApi.Validation
{
    public class ScooterValidator : AbstractValidator<Scooter>
    {
        public ScooterValidator()
        {
            RuleFor(x => x.Color).NotEmpty().MinimumLength(2);
        }
    }
}
