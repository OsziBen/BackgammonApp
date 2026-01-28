using Domain.GameLogic;
using Domain.GameLogic.Extensions;
using Domain.GameSession.Results;
using Domain.GameSession.Services;

namespace Domain.GameSession
{
    public partial class GameSession
    {
        public BoardState ApplyMoveSequence(
            BoardState boardState,
            MoveSequence moveSequence,
            Guid playerId,
            DateTimeOffset now,
            out GameOutcome? outcome)
        {
            EnsureCanMoveCheckers();
            EnsureCurrentPlayer(playerId);

            var nextState = boardState;

            foreach (var move in moveSequence.Moves)
            {
                nextState = nextState.Apply(move);
            }

            var winnerColor = GetPlayerColor(GetCurrentPlayer().Id);

            if (nextState.TryEvaluateVictory(
                winnerColor,
                out var resultType))
            {
                outcome = GameResultEvaluator.CreateOutcome(
                    resultType,
                    DoublingCubeValue);

                Finish(CurrentPlayerId!.Value, now);
            }
            else
            {
                outcome = null;
                EndTurn(now);
            }

            return nextState;
        }
    }
}
