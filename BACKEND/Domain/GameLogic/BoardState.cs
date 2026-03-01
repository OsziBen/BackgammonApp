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

        public BoardState With(
            Dictionary<int, CheckerPosition>? points = null,
            int? barWhite = null,
            int? barBlack = null,
            int? offWhite = null,
            int? offBlack = null,
            PlayerColor? currentPlayer = null)
        {
            return new BoardState(
                points ?? Points.ToDictionary(p => p.Key, p => p.Value.Clone()),
                barWhite ?? BarWhite,
                barBlack ?? BarBlack,
                offWhite ?? OffWhite,
                offBlack ?? OffBlack,
                currentPlayer ?? CurrentPlayer);
        }
    }
}
