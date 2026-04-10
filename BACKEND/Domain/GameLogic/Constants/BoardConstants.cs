using Common.Enums.BoardState;

namespace Domain.GameLogic.Constants
{
    public static class BoardConstants
    {
        // Bar / Off
        public const int BarPosition = 0;
        public const int OffBoardPosition = -1;

        public const int BoardPoints = 24;

        // Home board ranges
        public static readonly (int Start, int End) WhiteHome = (19, 24);
        public static readonly (int Start, int End) BlackHome = (1, 6);

        // Move direction
        public const int WhiteDirection = 1; // 1 → 24
        public const int BlackDirection = -1; // 24 → 1

        // Bear off positions
        public const int WhiteBearOff = 25;
        public const int BlackBearOff = 0;

        public static bool IsHomeBoard(
            int point,
            PlayerColor player)
        {
            return player == PlayerColor.White
                ? point >= WhiteHome.Start && point <= WhiteHome.End
                : point >= BlackHome.Start && point <= BlackHome.End;
        }

        public static bool IsEntryPoint(
            int point,
            PlayerColor player)
        {
            return player == PlayerColor.White
                ? point >= BlackHome.Start && point <= BlackHome.End
                : point >= WhiteHome.Start && point <= WhiteHome.End;
        }

        public static int GetDirection(PlayerColor player)
            => player == PlayerColor.White ? WhiteDirection : BlackDirection;

        public static int GetBearOffTarget(PlayerColor player)
            => player == PlayerColor.White ? WhiteBearOff : BlackBearOff;
    }
}
