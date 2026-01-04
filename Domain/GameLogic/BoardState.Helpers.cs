using Common.Enums.BoardState;
using Domain.GameLogic.Constants;

namespace Domain.GameLogic
{
    public partial class BoardState
    {
        public bool HasCheckersOnBar(PlayerColor player)
            => player == PlayerColor.White ? BarWhite > 0 : BarBlack > 0;

        public bool AllCheckersInHomeBoard(PlayerColor player)
            => Points.All(p =>
                p.Value.Owner != player ||
                BoardConstants.IsHomeBoard(p.Key, player));


        public bool HasCheckerFurtherFromBearOff(
            PlayerColor player,
            int fromPoint)
            => Points.Any(p =>
                p.Value.Owner == player &&
                p.Value.Count > 0 &&
                BoardConstants.IsHomeBoard(p.Key, player) &&
                (
                    (player == PlayerColor.White && p.Key > fromPoint) ||
                    (player == PlayerColor.Black && p.Key < fromPoint)
                ));

        public BoardState Clone()
            => new BoardState(
                ClonePoints(),
                BarWhite,
                BarBlack,
                OffWhite,
                OffBlack,
                CurrentPlayer);

        public Dictionary<int, CheckerPosition> ClonePoints()
            => Points.ToDictionary(p => p.Key, p => p.Value.Clone());

        public BoardState AddToBar(PlayerColor player)
        {
            return player == PlayerColor.White
                ? new BoardState(
                    Points.ToDictionary(p => p.Key, p => p.Value.Clone()),
                    BarWhite + 1,
                    BarBlack,
                    OffWhite,
                    OffBlack,
                    CurrentPlayer)
                : new BoardState(
                    Points.ToDictionary(p => p.Key, p => p.Value.Clone()),
                    BarWhite,
                    BarBlack + 1,
                    OffWhite,
                    OffBlack,
                    CurrentPlayer);
        }
    }
}
