using Common.Enums.BoardState;
using Domain.GameLogic;

namespace Application.Realtime
{
    public class RuntimeBoardStateSnapshot
    {
        public Dictionary<int, CheckerPosition> Points { get; set; } = [];
        public int BarWhite { get; set; }
        public int BarBlack { get; set; }
        public int OffWhite { get; set; }
        public int OffBlack { get; set; }
        public PlayerColor CurrentPlayer { get; set; }

        public static RuntimeBoardStateSnapshot FromDomain(BoardState state)
            => new()
            {
                Points = state.Points.ToDictionary(
                    p => p.Key,
                    p => p.Value.Clone()),
                BarWhite = state.BarWhite,
                BarBlack = state.BarBlack,
                OffWhite = state.OffWhite,
                OffBlack = state.OffBlack,
                CurrentPlayer = state.CurrentPlayer
            };

        public BoardState ToDomain()
            => new(
                Points,
                BarWhite,
                BarBlack,
                OffWhite,
                OffBlack,
                CurrentPlayer);
    }
}
