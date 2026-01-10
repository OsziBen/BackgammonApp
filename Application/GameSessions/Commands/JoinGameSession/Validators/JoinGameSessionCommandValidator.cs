using FluentValidation;

namespace Application.GameSessions.Commands.JoinGameSession.Validators
{
    public class JoinGameSessionCommandValidator : AbstractValidator<JoinGameSessionCommand>
    {
        public JoinGameSessionCommandValidator()
        {
            RuleFor(x => x.SessionCode)
                .NotEmpty()
                .WithMessage("Session code is required.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");

            RuleFor(x => x.ConnectionId)
                .NotEmpty()
                .WithMessage("Connection ID is required.");
        }
    }
}
