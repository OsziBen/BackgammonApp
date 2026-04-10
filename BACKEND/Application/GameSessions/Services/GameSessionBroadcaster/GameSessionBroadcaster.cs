using Application.GameSessions.Realtime;
using Application.GameSessions.Responses;
using Application.GameSessions.Services.GameSessionSnapshotFactory;
using Common.Enums.GameSession;
using Domain.GameSession;

namespace Application.GameSessions.Services.GameSessionBroadcaster
{
    public class GameSessionBroadcaster : IGameSessionBroadcaster
    {
        private readonly IGameSessionNotifier _gameSessionNotifier;
        private readonly IGameSessionSnapshotFactory _gameSessionSnapshotFactory;

        public GameSessionBroadcaster(
            IGameSessionNotifier gameSessionNotifier,
            IGameSessionSnapshotFactory gameSessionSnapshotFactory)
        {
            _gameSessionNotifier = gameSessionNotifier;
            _gameSessionSnapshotFactory = gameSessionSnapshotFactory;
        }

        public async Task BroadcastAsync(GameSession session, SessionEventType eventType, bool isRejoin = false)
        {
            foreach (var player in session.Players)
            {
                var snapshot = _gameSessionSnapshotFactory.Create(
                    session,
                    localPlayerId: player.Id,
                    isRejoin: isRejoin);

                await _gameSessionNotifier.SessionUpdated(
                    player.UserId,
                    new SessionUpdatedMessage
                    {
                        EventType = eventType,
                        Snapshot = snapshot
                    });
            }
        }
    }
}
