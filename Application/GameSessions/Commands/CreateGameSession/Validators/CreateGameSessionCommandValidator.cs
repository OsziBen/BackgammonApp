using FluentValidation;

namespace Application.GameSessions.Commands.CreateGameSession.Validators
{
    public class CreateGameSessionCommandValidator : AbstractValidator<CreateGameSessionCommand>
    {
        public CreateGameSessionCommandValidator()
        {
            RuleFor(x => x.HostPlayerId)
                .NotEmpty()
                .WithMessage("Host player is required.");

            RuleFor(x => x.Settings)
                .NotNull()
                .WithMessage("Game session settings are required.");

            When(x => x.Settings != null, () =>
            {
                RuleFor(x => x.Settings.TargerPoints)
                    .GreaterThan(0)
                    .WithMessage("Target points must be greater than zero.");

                RuleFor(x => x.Settings)
                    .Must(s => !s.ClockEnabled || s.MatchTimePerPlayerInSeconds.HasValue)
                    .WithMessage("Match time per player is required when clock is enabled.");

                RuleFor(x => x.Settings)
                    .Must(s => !s.ClockEnabled || s.MatchTimePerPlayerInSeconds > 0)
                    .WithMessage("Match time per player must be greater than zero.");

                RuleFor(x => x.Settings)
                    .Must(s => !s.ClockEnabled ||
                               !s.StartOfTurnDelayPerPlayerInSeconds.HasValue ||
                               s.StartOfTurnDelayPerPlayerInSeconds >= 0)
                    .WithMessage("Start of turn delay must be zero or positive.");
            });
        }
    }
}
