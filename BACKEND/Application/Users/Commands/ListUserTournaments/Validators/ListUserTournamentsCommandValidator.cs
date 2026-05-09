using FluentValidation;

namespace Application.Users.Commands.ListUserTournaments.Validators
{
    public class ListUserTournamentsCommandValidator : AbstractValidator<ListUserTournamentsCommand>
    {
        public ListUserTournamentsCommandValidator()
        {
            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("User ID is required.");
        }
    }
}
