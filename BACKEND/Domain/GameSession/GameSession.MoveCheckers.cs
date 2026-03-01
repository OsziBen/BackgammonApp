using Common.Enums.GameSession;
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
            EnsureCanMoveCheckers(playerId);

            var nextState = boardState;

            foreach (var move in moveSequence.Moves)
            {
                nextState = nextState.Apply(move);
            }

            if (nextState.TryEvaluateVictory(
                GetPlayerOrThrow(CurrentPlayerId!.Value).Color,
                out var resultType))
            {
                outcome = GameResultEvaluator.CreateOutcome(
                    resultType!.Value,
                    DoublingCubeValue);

                Finish(GameFinishReason.Victory, CurrentPlayerId!.Value, now);
            }
            else
            {
                outcome = null;
                EndTurn(now);
                nextState = nextState.With(currentPlayer: GetPlayerOrThrow(CurrentPlayerId.Value).Color);
            }

            return nextState;
        }
    }
}
