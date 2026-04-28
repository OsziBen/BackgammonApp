using FluentValidation;

namespace Application.Groups.Commands.JoinGroup.Validators
{
    public class JoinGroupCommandValidator : AbstractValidator<JoinGroupCommand>
    {
        public JoinGroupCommandValidator()
        {
            RuleFor(x => x.GroupId)
               .NotEmpty()
               .WithMessage("Group ID is required.");

            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("User ID is required.");
        }
    }
}
