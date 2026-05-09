using FluentValidation;

namespace Application.Groups.Commands.ListGroupMembers.Validators
{
    public class ListGroupMembersCommandValidator : AbstractValidator<ListGroupMembersCommand>
    {
        public ListGroupMembersCommandValidator()
        {
            RuleFor(x => x.GroupId)
               .NotEmpty()
               .WithMessage("Group ID is required.");
        }
    }
}
