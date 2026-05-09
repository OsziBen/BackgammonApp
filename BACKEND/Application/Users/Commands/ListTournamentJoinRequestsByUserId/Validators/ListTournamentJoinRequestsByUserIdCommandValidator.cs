using FluentValidation;

namespace Application.Users.Commands.ListTournamentJoinRequestsByUserId.Validators
{
    public class ListTournamentJoinRequestsByUserIdCommandValidator : AbstractValidator<ListTournamentJoinRequestsByUserIdCommand>
    {
        public ListTournamentJoinRequestsByUserIdCommandValidator()
        {
            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("User ID is required.");
        }
    }
}
