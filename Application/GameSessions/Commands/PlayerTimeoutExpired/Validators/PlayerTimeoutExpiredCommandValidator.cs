using FluentValidation;

namespace Application.GameSessions.Commands.PlayerTimeoutExpired.Validators
{
    public class PlayerTimeoutExpiredCommandValidator : AbstractValidator<PlayerTimeoutExpiredCommand>
    {
        public PlayerTimeoutExpiredCommandValidator()
        {

        }
    }
}
