using Application.Groups.Commands.PromoteGroupMemberToModerator;
using FluentValidation;

namespace Application.Groups.Commands.PromoteGroupMemberToModerator.Validators
{
    public class PromoteGroupMemberToModeratorCommandValidator : AbstractValidator<PromoteGroupMemberToModeratorCommand>
    {
        public PromoteGroupMemberToModeratorCommandValidator()
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
