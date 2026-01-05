using Common.Enums.BoardState;
using Domain.GameLogic.Constants;

namespace Domain.GameLogic
{
    public partial class BoardState
    {
        public bool CanBearOff(PlayerColor player)
            => AllCheckersInHomeBoard(player);

        public bool IsValidBearOffMove(
            PlayerColor player,
            int fromPoint,
            int die)
        {
            if (!CanBearOff(player))
            {
                return false;
            }

            var target = player == PlayerColor.White
                ? fromPoint + die
                : fromPoint - die;

            if (target == BoardConstants.OffBoardPosition)
            {
                return true;
            }

            return !HasCheckerFurtherFromBearOff(player, fromPoint);
        }
    }
}
