using FluentValidation;

namespace Application.Tournaments.Commands.JoinTournament.Validators
{
    public class JoinTournamentCommandValidator : AbstractValidator<JoinTournamentCommand>
    {
        public JoinTournamentCommandValidator()
        {
            RuleFor(x => x.TournamentId)
               .NotEmpty()
               .WithMessage("Tournament ID is required.");

            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("User ID is required.");
        }
    }
}
