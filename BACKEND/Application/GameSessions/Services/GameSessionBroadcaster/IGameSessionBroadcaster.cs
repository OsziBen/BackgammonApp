using Common.Enums.GameSession;
using Domain.GameSession;

namespace Application.GameSessions.Services.GameSessionBroadcaster
{
    public interface IGameSessionBroadcaster
    {
        Task BroadcastAsync(
            GameSession session,
            SessionEventType eventType,
            bool isRejoin = false);
    }
}
