using Application.GameSessions.Responses;

namespace Application.GameSessions.Realtime
{
    public interface IGameSessionNotifier
    {
        Task SessionUpdated(
            Guid sessionId,
            SessionUpdatedMessage sessionUpdatedMessage);
    }
}
