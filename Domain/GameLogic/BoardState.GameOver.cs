using Common.Enums.BoardState;
using Common.Enums.Game;

namespace Domain.GameLogic
{
    public partial class BoardState
    {
        public bool IsGameOver(
            out PlayerColor winner,
            out GameResultType resultType)
        {
            if (OffWhite == 15)
            {
                winner = PlayerColor.White;
                resultType = EvaluateResult(PlayerColor.Black);

                return true;
            }

            if (OffBlack == 15)
            {
                winner = PlayerColor.Black;
                resultType = EvaluateResult(PlayerColor.White);
                return true;
            }

            winner = default;
            resultType = default;

            return false;
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
