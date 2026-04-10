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
            var player = state.CurrentPlayer;

            if (!state.AllCheckersInHomeBoard(player))
            {
                return false;
            }

            var direction = BoardConstants.GetDirection(player);
            var exactTarget = fromPoint + (die * direction);

            var bearOffTarget = BoardConstants.GetBearOffTarget(player);

            if (exactTarget == bearOffTarget)
            {
                return true;
            }

            var hasFurther = state.HasCheckerFurtherFromBearOff(player, fromPoint);

            bool overshoot = direction > 0
                ? exactTarget > BoardConstants.BoardPoints
                : exactTarget < 1;

            if (overshoot && !hasFurther)
            {
                return true;
            }

            return false;
        }
    }
}
