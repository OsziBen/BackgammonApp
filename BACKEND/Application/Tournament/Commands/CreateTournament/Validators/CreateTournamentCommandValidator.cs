using FluentValidation;

namespace Application.Tournament.Commands.CreateTournament.Validators
{
    public class CreateTournamentCommandValidator : AbstractValidator<CreateTournamentCommand>
    {
        public CreateTournamentCommandValidator()
        {
        }
    }
}
