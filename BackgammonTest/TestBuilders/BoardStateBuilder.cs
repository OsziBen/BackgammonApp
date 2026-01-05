using Common.Enums.BoardState;
using Domain.GameLogic;

namespace BackgammonTest.TestBuilders
{
    public class BoardStateBuilder
    {
        private readonly Dictionary<int, CheckerPosition> _points = new();
        private int _barWhite;
        private int _barBlack;
        private int _offWhite;
        private int _offBlack;
        private PlayerColor _currentPlayer = PlayerColor.White;

        public static BoardStateBuilder Default()
            => new BoardStateBuilder();

        public BoardStateBuilder WithCurrentPlayer(PlayerColor player)
        {
            _currentPlayer = player;

            return this;
        }

        public BoardStateBuilder WithChecker(
            int point,
            PlayerColor owner,
            int count = 1)
        {
            _points[point] = new CheckerPosition(owner, count);

            return this;
        }

        public BoardStateBuilder WithEnemyChecker(
            int point,
            PlayerColor owner,
            int count = 1)
        {
            if (owner == PlayerColor.White)
            {
                _points[point] = new CheckerPosition(PlayerColor.Black, count);
            }
            else
            {
                _points[point] = new CheckerPosition(PlayerColor.White, count);
            }

            return this;
        }

        public BoardStateBuilder WithBar(
            PlayerColor player,
            int count)
        {
            if (player == PlayerColor.White)
                _barWhite = count;
            else
                _barBlack = count;

            return this;
        }

        public BoardStateBuilder WithOff(
            PlayerColor player,
            int count)
        {
            if (player == PlayerColor.White)
                _offWhite = count;
            else
                _offBlack = count;

            return this;
        }

        public BoardState Build()
        {
            for (int i = 1; i <= 24; i++)
            {
                if (!_points.ContainsKey(i))
                {
                    _points[i] = new CheckerPosition(null, 0);
                }
            }

            return new BoardState(
                _points,
                _barWhite,
                _barBlack,
                _offWhite,
                _offBlack,
                _currentPlayer);
        }
    }
}
