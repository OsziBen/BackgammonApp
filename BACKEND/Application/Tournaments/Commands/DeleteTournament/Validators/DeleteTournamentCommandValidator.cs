using FluentValidation;

namespace Application.Tournaments.Commands.DeleteTournament.Validators
{
    public class DeleteTournamentCommandValidator : AbstractValidator<DeleteTournamentCommand>
    {
        public DeleteTournamentCommandValidator()
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
