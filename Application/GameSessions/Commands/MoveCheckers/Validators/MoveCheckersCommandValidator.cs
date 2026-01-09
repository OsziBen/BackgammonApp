using FluentValidation;

namespace Application.GameSessions.Commands.MoveCheckers.Validators
{
    public class MoveCheckersCommandValidator : AbstractValidator<MoveCheckersCommand>
    {
        public MoveCheckersCommandValidator()
        {
            RuleFor(x => x.SessionId)
                .NotEmpty()
                .WithMessage("SessionId must not be empty.");

            RuleFor(x => x.PlayerId)
                .NotEmpty()
                .WithMessage("PlayerId must not be empty.");

            RuleFor(x => x.Moves)
                .NotEmpty()
                .WithMessage("Moves list must not be empty.");

            RuleForEach(x => x.Moves)
                .SetValidator(new MoveDtoValidator());
        }
    }
}
