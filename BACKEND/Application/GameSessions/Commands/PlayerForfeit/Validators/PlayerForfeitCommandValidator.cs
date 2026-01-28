using FluentValidation;

namespace Application.GameSessions.Commands.PlayerForfeit.Validators
{
    public class PlayerForfeitCommandValidator : AbstractValidator<PlayerForfeitCommand>
    {
        public PlayerForfeitCommandValidator()
        {
            RuleFor(x => x.SessionId)
                .NotEmpty()
                .WithMessage("SessionId must not be empty.");

            RuleFor(x => x.PlayerId)
                .NotEmpty()
                .WithMessage("PlayerId must not be empty.");
        }
    }
}
