using Application.GameSessions.Requests;
using Domain.GameLogic.Constants;
using FluentValidation;

namespace Application.GameSessions.Commands.MoveCheckers.Validators
{
    public class MoveDtoValidator : AbstractValidator<MoveDto>
    {
        public MoveDtoValidator()
        {
            RuleFor(x => x.From)
                .InclusiveBetween(BoardConstants.BarPosition, BoardConstants.BoardPoints)
                .WithMessage($"From point must be between {BoardConstants.BarPosition} (Bar) and {BoardConstants.BoardPoints}");

            RuleFor(x => x.To)
                .InclusiveBetween(BoardConstants.OffBoardPosition, BoardConstants.BoardPoints)
                .Must(to => to != 0)
                .WithMessage($"To point cannot be {BoardConstants.BarPosition}; use {BoardConstants.OffBoardPosition} for Off or 1-{BoardConstants.BoardPoints} for board points");

            RuleFor(x => x.Die)
                .InclusiveBetween(1, 6)
                .WithMessage("Die value must be between 1 and 6");
        }
    }
}
