using Domain.GameSession;

namespace Application.GameSessions.Responses
{
    public class GetActiveSessionByUserIdResponse
    {
        public Guid SessionId { get; set; }
        public int Version { get; set; }
        public required string SessionCode { get; set; }
        public required GameSessionSettings Settings { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
