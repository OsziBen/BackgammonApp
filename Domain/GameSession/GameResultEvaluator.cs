using Common.Enums.Game;
using Domain.GameSession.Results;

namespace Domain.GameSession
{
    public static class GameResultEvaluator
    {
        public static GameOutcome CreateOutcome(
            GameResultType resultType,
            int? doublingCubeValue)
        {
            var basePoints = resultType switch
            {
                GameResultType.SimpleVictory => 1,
                GameResultType.GammonVictory => 2,
                GameResultType.BackgammonVictory => 3,
                _ => throw new ArgumentOutOfRangeException()
            };

            var multiplier = doublingCubeValue ?? 1;

            return new GameOutcome(
                resultType,
                basePoints * multiplier);
        }
    }
}
