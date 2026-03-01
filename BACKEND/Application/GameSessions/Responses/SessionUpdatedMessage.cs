using Common.Enums.GameSession;

namespace Application.GameSessions.Responses
{
    public class SessionUpdatedMessage
    {
        public SessionEventType EventType { get; init; }
        public required GameSessionSnapshotResponse Snapshot { get; init; }
    }
}
