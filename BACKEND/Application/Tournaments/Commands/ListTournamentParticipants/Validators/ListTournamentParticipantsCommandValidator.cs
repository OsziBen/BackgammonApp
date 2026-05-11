using FluentValidation;

namespace Application.Tournaments.Commands.ListTournamentParticipants.Validators
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
