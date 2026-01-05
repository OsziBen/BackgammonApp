using Common.Enums.BoardState;

namespace Domain.GameLogic.Constants
{
    public static class BoardConstants
    {
        public const int BarPosition = 0;
        public const int OffBoardPosition = -1;

        public static bool IsHomeBoard(
            int point,
            PlayerColor player)
        {
            return player == PlayerColor.White
                ? point >= 19 && point <= 24
                : point >= 1 && point <= 6;
        }

        public static bool IsEntryPoint(
            int point,
            PlayerColor player)
        {
            return player == PlayerColor.White
                ? point >= 1 && point <= 6
                : point >= 19 && point <= 24;
        }
    }
}
