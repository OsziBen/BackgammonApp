using FluentValidation;

namespace Application.Groups.Commands.ListMembers.Validators
{
    public class ListMembersCommandValidator : AbstractValidator<ListMembersCommand>
    {
        public ListMembersCommandValidator()
        {
            RuleFor(x => x.GroupId)
               .NotEmpty()
               .WithMessage("Group ID is required.");
        }
    }
}
