using FluentValidation;

namespace Application.Tournament.Commands.RemoveTournamentParticipant.Validators
{
    public class RemoveTournamentParticipantCommandValidator : AbstractValidator<RemoveTournamentParticipantCommand>
    {
        public RemoveTournamentParticipantCommandValidator()
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
