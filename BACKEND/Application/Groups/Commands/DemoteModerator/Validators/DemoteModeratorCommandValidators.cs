using FluentValidation;

namespace Application.Groups.Commands.DemoteModerator.Validators
{
    public class DemoteModeratorCommandValidators : AbstractValidator<DemoteModeratorCommand>
    {
        public DemoteModeratorCommandValidators()
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
