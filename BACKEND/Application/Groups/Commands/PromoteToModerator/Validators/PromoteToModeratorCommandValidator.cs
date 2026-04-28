using FluentValidation;

namespace Application.Groups.Commands.PromoteToModerator.Validators
{
    public class PromoteToModeratorCommandValidator : AbstractValidator<PromoteToModeratorCommand>
    {
        public PromoteToModeratorCommandValidator()
        {
            RuleFor(x => x.GroupId)
               .NotEmpty()
               .WithMessage("Group ID is required.");

            RuleFor(x => x.TargetUserId)
               .NotEmpty()
               .WithMessage("Target User ID is required.");

            RuleFor(x => x.CurrentUserId)
               .NotEmpty()
               .WithMessage("Current User ID is required.");
        }
    }
}
