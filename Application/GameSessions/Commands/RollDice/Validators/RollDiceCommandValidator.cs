using FluentValidation;

namespace Application.GameSessions.Commands.RollDice.Validators
{
    public class RollDiceCommandValidator : AbstractValidator<RollDiceCommand>
    {
        public RollDiceCommandValidator()
        {
            RuleFor(x => x.SessionId)
                .NotEmpty();

            RuleFor(x => x.PlayerId)
                .NotEmpty();
        }
    }
}
