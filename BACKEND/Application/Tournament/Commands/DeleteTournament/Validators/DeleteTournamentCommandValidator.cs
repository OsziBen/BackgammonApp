using FluentValidation;

namespace Application.Tournament.Commands.DeleteTournament.Validators
{
    public class DeleteTournamentCommandValidator : AbstractValidator<DeleteTournamentCommand>
    {
        public DeleteTournamentCommandValidator()
        {
        }
    }
}
