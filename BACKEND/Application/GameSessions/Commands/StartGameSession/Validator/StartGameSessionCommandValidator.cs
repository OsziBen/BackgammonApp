using FluentValidation;

namespace Application.GameSessions.Commands.StartGameSession.Validator
{
    public class StartGameSessionCommandValidator : AbstractValidator<StartGameSessionCommand>
    {
        public StartGameSessionCommandValidator()
        {
            RuleFor(x => x.SessionId)
                .NotEmpty()
                .WithMessage("Session ID is required.");
        }
    }
}
