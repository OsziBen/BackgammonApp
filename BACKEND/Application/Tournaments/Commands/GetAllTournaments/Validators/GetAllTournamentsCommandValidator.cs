using FluentValidation;

namespace Application.Tournaments.Commands.GetAllTournaments.Validators
{
    public class GetAllTournamentsCommandValidator : AbstractValidator<GetAllTournamentsCommand>
    {
        public GetAllTournamentsCommandValidator()
        {
            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("User ID is required.");
        }
    }
}
