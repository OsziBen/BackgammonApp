using Common.Enums.BoardState;
using Common.Enums.Game;

namespace Domain.GameLogic
{
    public partial class BoardState
    {
        public bool TryEvaluateVictory(
            PlayerColor winner,
            out GameResultType resultType)
        {
            var loser = winner == PlayerColor.White
                ? PlayerColor.Black
                : PlayerColor.White;

            if (!HasWinnerCheckersOff(winner))
            {
                resultType = default;

                return false;
            }

            resultType = EvaluateResult(loser);

            return true;
        }

        private GameResultType EvaluateResult(PlayerColor loser)
        {
            if ((loser == PlayerColor.White ? OffWhite : OffBlack) > 0)
            {
                return GameResultType.SimpleVictory;
            }

            if (HasCheckersOnBar(loser) || HasCheckerInHomeBoard(loser))
            {
                return GameResultType.BackgammonVictory;
            }

            return GameResultType.GammonVictory;
        }

        private bool HasCheckerInHomeBoard(PlayerColor player)
            => Points.Any(p =>
                p.Value.Owner == player &&
                IsInHomeBoard(player, p.Key));

        private static bool IsInHomeBoard(
            PlayerColor player,
            int point)
            => player == PlayerColor.White ? point >= 19 : point <= 6;
    }
}
