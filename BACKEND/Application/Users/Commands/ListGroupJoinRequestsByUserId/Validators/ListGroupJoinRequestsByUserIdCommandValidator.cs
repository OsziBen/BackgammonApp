using FluentValidation;

namespace Application.Users.Commands.ListGroupJoinRequestsByUserId.Validators
{
    public class ListGroupJoinRequestsByUserIdCommandValidator : AbstractValidator<ListGroupJoinRequestsByUserIdCommand>
    {
        public ListGroupJoinRequestsByUserIdCommandValidator()
        {
            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("User ID is required.");
        }
    }
}
