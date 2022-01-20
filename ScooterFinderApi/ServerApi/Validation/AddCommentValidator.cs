using FluentValidation;
using ServerApi.DTO.Comment;

namespace ServerApi.Validation
{
    public class AddCommentValidator : AbstractValidator<AddCommentRequest>
    {
        public AddCommentValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.PinId).NotEmpty();
            RuleFor(x => x.Content).MaximumLength(2048);
        }
    }
}
