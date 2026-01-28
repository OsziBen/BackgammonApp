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

        public GameResultType EvaluateForfeitResult(
            PlayerColor forfeitingPlayer)
        {
            return EvaluateResult(forfeitingPlayer);
        }

        private GameResultType EvaluateResult(PlayerColor loser)
        {
            if (HasAnyCheckersOff(loser))
            {
                return GameResultType.SimpleVictory;
            }

            if (HasCheckersOnBar(loser) || HasCheckerInHomeBoard(loser))
            {
                return GameResultType.BackgammonVictory;
            }

            return GameResultType.GammonVictory;
        }

        private static bool IsInHomeBoard(PlayerColor player, int point)
            => player == PlayerColor.White ? point >= 19 : point <= 6;

        private bool HasCheckerInHomeBoard(PlayerColor player)
            => Points.Any(p =>
                p.Value.Owner == player &&
                IsInHomeBoard(player, p.Key));

        private bool HasAnyCheckersOff(PlayerColor player)
             => player == PlayerColor.White
                 ? OffWhite > 0
                 : OffBlack > 0;

        private bool HasWinnerCheckersOff(PlayerColor winner)
            => winner == PlayerColor.White
                ? OffWhite == 15
                : OffBlack == 15;
    }
}
