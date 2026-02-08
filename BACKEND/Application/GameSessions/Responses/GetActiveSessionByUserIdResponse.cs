using Domain.GameSession;

namespace Application.GameSessions.Responses
{
    public class GetActiveSessionByUserIdResponse
    {
        public Guid SessionId { get; set; }
        public string SessionCode { get; set; } = null!;
        public GameSessionSettings Settings { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
