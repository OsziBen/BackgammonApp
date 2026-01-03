using Common.Enums.BoardState;

namespace Domain.GameLogic
{
    public partial class BoardState
    {
        public IReadOnlyDictionary<int, CheckerPosition> Points { get; }
        public int BarWhite { get; }
        public int BarBlack { get; }
        public int OffWhite { get; }
        public int OffBlack { get; }
        public PlayerColor CurrentPlayer { get; }

        public BoardState(
            Dictionary<int, CheckerPosition> points,
            int barWhite,
            int barBlack,
            int offWhite,
            int offBlack,
            PlayerColor currentPlayer)
        {
            Points = points;
            BarWhite = barWhite;
            BarBlack = barBlack;
            OffWhite = offWhite;
            OffBlack = offBlack;
            CurrentPlayer = currentPlayer;
        }
    }
}
