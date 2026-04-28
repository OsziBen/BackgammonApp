using FluentValidation;

namespace Application.Groups.Commands.RemoveGroupMember.Validators
{
    public class RemoveGroupMemberCommandValidator : AbstractValidator<RemoveGroupMemberCommand>
    {
        public RemoveGroupMemberCommandValidator()
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
