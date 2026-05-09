using FluentValidation;

namespace Application.Tournament.Commands.ListTournamentParticipants.Validators
{
    public class ListTournamentParticipantsCommandValidator : AbstractValidator<ListTournamentParticipantsCommand>
    {
        public ListTournamentParticipantsCommandValidator()
        {
            RuleFor(x => x.TournamentId)
               .NotEmpty()
               .WithMessage("Tournament ID is required.");
        }
    }
}
