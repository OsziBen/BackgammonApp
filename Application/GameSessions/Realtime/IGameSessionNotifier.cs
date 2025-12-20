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
    }
}
