using Common.Enums.Game;
using Domain.GameLogic;
using Domain.GameLogic.Extensions;

namespace Domain.GameSession
{
    public partial class GameSession
    {
        public BoardState ApplyMoveSequence(
            BoardState boardState,
            MoveSequence moveSequence,
            Guid playerId,
            DateTimeOffset now,
            out GameResultType? resultType)
        {
            EnsureCanMoveCheckers();
            EnsureCurrentPlayer(playerId);

            var nextState = boardState;

            foreach (var move in moveSequence.Moves)
            {
                nextState = nextState.Apply(move);
            }

            var currentPlayer = GetCurrentPlayer();
            var winnerColor = currentPlayer.Color;

            if (nextState.TryEvaluateVictory(
                winnerColor,
                out var gameResult))
            {
                resultType = GameResultEvaluator.Evaluate(
                    nextState,
                    winnerColor,
                    DoublingCubeValue);

                Finish(CurrentPlayerId!.Value, now);
            }
            else
            {
                resultType = null;
                EndTurn(now);
            }

            return nextState;
        }
    }
}
