using Common.Enums.BoardState;
using Domain.GameLogic.Constants;

namespace Domain.GameLogic.Rules
{
    public static class BearOffRules
    {
        public static bool CanBearOff(
            BoardState state,
            int fromPoint,
            int die)
        {
            if (!state.AllCheckersInHomeBoard(state.CurrentPlayer))
            {
                return false;
            }

            var exactTarget = state.CurrentPlayer == PlayerColor.White
                ? fromPoint + die
                : fromPoint - die;

            if (exactTarget == BoardConstants.OffBoardPosition)
            {
                return true;
            }

            return !state.HasCheckerOnHigherPoint(
                state.CurrentPlayer,
                fromPoint);
        }
    }
}
