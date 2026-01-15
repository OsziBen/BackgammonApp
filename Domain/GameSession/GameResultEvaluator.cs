using Common.Enums.BoardState;
using Common.Enums.Game;
using Domain.GameLogic;

namespace Domain.GameSession
{
    public static class GameResultEvaluator
    {
        public static GameResultType Evaluate(
            BoardState boardState,
            PlayerColor winnerColor,
            int? doublingCubeValue)
        {
            if (!boardState.TryEvaluateVictory(winnerColor, out var baseResult))
            {
                baseResult = GameResultType.SimpleVictory;
            }

            return ApplyDoublingCube(baseResult, doublingCubeValue);
        }

        private static GameResultType ApplyDoublingCube(
            GameResultType result,
            int? cubeValue)
        {
            if (cubeValue == null || cubeValue == 1)
            {
                return result;
            }

            // TODO: multiply logic

            return result;
        }
    }
}
