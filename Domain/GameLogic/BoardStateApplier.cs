using Common.Enums.BoardState;
using Domain.GameLogic.Constants;

namespace Domain.GameLogic
{
    public static class BoardStateApplier
    {
        public static BoardState Apply(
            BoardState state,
            Move move)
        {
            var points = state.ClonePoints();

            var barWhite = state.BarWhite;
            var barBlack = state.BarBlack;
            var offWhite = state.OffWhite;
            var offBlack = state.OffBlack;

            // FROM
            var from = points[move.From];
            points[move.From] = from.Count == 1
                ? new CheckerPosition(null, 0)
                : new CheckerPosition(from.Owner, from.Count - 1);

            // BEAR OFF
            if (move.To == BoardConstants.OffBoardPosition)
            {
                if (state.CurrentPlayer == PlayerColor.White)
                {
                    offWhite++;
                }
                else
                {
                    offBlack++;
                }

                return new BoardState(
                    points,
                    barWhite,
                    barBlack,
                    offWhite,
                    offBlack,
                    state.CurrentPlayer);
            }

            // NORMAL MOVE
            var target = points[move.To];

            // HIT
            if (target.Owner != null &&
                target.Owner != state.CurrentPlayer &&
                target.Count == 1)
            {
                if (target.Owner == PlayerColor.White)
                {
                    barWhite++;
                }
                else
                {
                    barBlack++;
                }

                target = new CheckerPosition(null, 0);
            }

            points[move.To] = new CheckerPosition(
                state.CurrentPlayer,
                target.Count + 1);

            return new BoardState(
                points,
                barWhite,
                barBlack,
                offWhite,
                offBlack,
                state.CurrentPlayer);
        }
    }
}
