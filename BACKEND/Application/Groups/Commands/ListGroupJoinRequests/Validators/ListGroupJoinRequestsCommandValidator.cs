using FluentValidation;

namespace Application.Groups.Commands.ListGroupJoinRequests.Validators
{
    public class ListGroupJoinRequestsCommandValidator : AbstractValidator<ListGroupJoinRequestsCommand>
    {
        public ListGroupJoinRequestsCommandValidator()
        {
            RuleFor(x => x.GroupId)
               .NotEmpty()
               .WithMessage("Group ID is required.");
        }
    }
}
