using FluentValidation;

namespace Application.Tournament.Commands.UpdateTournament.Validators
{
    public class UpdateTournamentCommandValidator : AbstractValidator<UpdateTournamentCommand>
    {
        public UpdateTournamentCommandValidator()
        {
        }
    }
}
