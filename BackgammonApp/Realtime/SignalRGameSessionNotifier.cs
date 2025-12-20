using Application.GameSessions.Realtime;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Realtime
{
    public class SignalRGameSessionNotifier : IGameSessionNotifier
    {
        private readonly IHubContext<GameSessionHub> _hub;

        public SignalRGameSessionNotifier(IHubContext<GameSessionHub> hub)
        {
            _hub = hub;
        }

        public async Task PlayerDisconnected(
            Guid sessionId,
            Guid playerId,
            DateTimeOffset disconnectedAt)
        {
            await _hub.Clients
                .Group(sessionId.ToString())
                .SendAsync("PlayerDisconnected", new
                {
                    PlayerId = playerId,
                    DisconnectedAt = disconnectedAt
                });
        }

        public async Task PlayerReconnected(
            Guid sessionId,
            Guid playerId,
            DateTimeOffset reconnectedAt)
        {
            await _hub.Clients
                .Group(sessionId.ToString())
                .SendAsync("PlayerReconnected", new
                {
                    PlayerId = playerId,
                    ReconnectedAt = reconnectedAt
                });
        }

        public async Task PlayerTimeoutExpired(
            Guid sessionId,
            Guid timedOutPlayerId,
            Guid? winnerPlayerId)
        {
            await _hub.Clients
                .Group(sessionId.ToString())
                .SendAsync("PlayerTimeoutExpired", new
                {
                    TimedOutPlayerId = timedOutPlayerId,
                    WinnerPlayerId = winnerPlayerId
                });
        }
    }
}
