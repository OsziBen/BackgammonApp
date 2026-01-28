using Application.GameSessions.Requests;
using FluentValidation;

namespace Application.GameSessions.Commands.MoveCheckers.Validators
{
    public class MoveDtoValidator : AbstractValidator<MoveDto>
    {
        public MoveDtoValidator()
        {
            RuleFor(x => x.From)
                .InclusiveBetween(0, 24)
                .WithMessage("From point must be between 0 (Bar) and 24");

            RuleFor(x => x.To)
                .InclusiveBetween(-1, 24)
                .Must(to => to != 0)
                .WithMessage("To point cannot be 0; use -1 for Off or 1-24 for board points");

            RuleFor(x => x.Die)
                .InclusiveBetween(1, 6)
                .WithMessage("Die value must be between 1 and 6");
        }
    }
}
