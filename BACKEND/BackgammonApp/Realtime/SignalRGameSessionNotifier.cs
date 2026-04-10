using Application.GameSessions.Realtime;
using Application.GameSessions.Responses;
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

        public Task SessionUpdated(Guid playerId, SessionUpdatedMessage sessionUpdatedMessage)
            => _hub.Clients
                .User(playerId.ToString())
                .SendAsync(
                    "SessionUpdated",
                    sessionUpdatedMessage
                );
    }
}
