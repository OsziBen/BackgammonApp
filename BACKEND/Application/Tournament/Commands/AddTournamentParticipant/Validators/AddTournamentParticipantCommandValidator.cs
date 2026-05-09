using FluentValidation;

namespace Application.Tournament.Commands.AddTournamentParticipant.Validators
{
    public class AddTournamentParticipantCommandValidator : AbstractValidator<AddTournamentParticipantCommand>
    {
        public AddTournamentParticipantCommandValidator()
        {
            RuleFor(x => x.TournamentId)
               .NotEmpty()
               .WithMessage("Group ID is required.");

            RuleFor(x => x.UserName)
               .NotEmpty()
               .WithMessage("User Name is required.");

            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("User ID is required.");
        }
    }
}
