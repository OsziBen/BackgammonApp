using Common.Enums.BoardState;
using Common.Enums.GameSession;
using Domain.GameLogic;

namespace Application.Realtime
{
    public sealed class RuntimeBoardStateSnapshot
    {
        public Dictionary<int, CheckerPosition> Points { get; init; } = [];
        public int BarWhite { get; init; }
        public int BarBlack { get; init; }
        public int OffWhite { get; init; }
        public int OffBlack { get; init; }
        public PlayerColor CurrentPlayer { get; init; }
        public GamePhase CurrentGamePhase { get; init; }
        public IReadOnlyList<int>? DiceRoll { get; init; }
    }
}
