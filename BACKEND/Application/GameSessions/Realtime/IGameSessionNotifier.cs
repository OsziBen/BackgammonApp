using Application.GameSessions.Requests;
using Common.Enums.Game;
using Common.Enums.GameSession;
using Domain.GameSession.Results;

namespace Application.GameSessions.Realtime
{
    public interface IGameSessionNotifier
    {
        Task PlayerDisconnected(
            Guid sessionId,
            Guid playerId,
            DateTimeOffset disconnectedAt);

        Task PlayerReconnected(
            Guid sessionId,
            Guid playerId,
            DateTimeOffset reconnectedAt);

        Task PlayerTimeoutExpired(
                Guid sessionId,
                Guid timedOutPlayerId,
                Guid? winnerPlayerId);

        Task StartingPlayerDetermined(
            Guid sessionId,
            IEnumerable<(Guid PlayerId, int Roll)> rolls,
            Guid startingPlayerId);

        Task GameStarted(
            Guid sessionId,
            Guid startingPlayerId);

        Task DiceRolled(
            Guid sessionId,
            Guid playerId,
            int die1,
            int die2);

        Task CheckersMoved(
            Guid sessionId,
            Guid playerId,
            IReadOnlyList<MoveDto> moves);

        Task GameFinished(
            Guid sessionId,
            Guid winnerPlayerId,
            GameFinishReason reason,
            GameOutcome outcome);

        Task DoublingCubeOffered(
            Guid sessionId,
            Guid playerId,
            int cubeValue);

        Task DoublingCubeAccepted(
            Guid sessionId,
            Guid acceptingPlayerId,
            int cubeValue);
    }
}
