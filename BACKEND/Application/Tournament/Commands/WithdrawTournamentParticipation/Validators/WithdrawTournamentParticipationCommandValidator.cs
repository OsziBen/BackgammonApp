using FluentValidation;

namespace Application.Tournament.Commands.WithdrawTournamentParticipation.Validators
{
    public class WithdrawTournamentParticipationCommandValidator : AbstractValidator<WithdrawTournamentParticipationCommand>
    {
        public WithdrawTournamentParticipationCommandValidator()
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
