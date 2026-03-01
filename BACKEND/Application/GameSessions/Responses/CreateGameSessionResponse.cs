using Common.Enums.GameSession;
using Domain.GameSession;

namespace Application.GameSessions.Responses
{
    public class CreateGameSessionResponse
    {
        public Guid SessionId { get; set; }
        public required string SessionCode { get; set; }
        public required GameSessionSettings Settings { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public GamePhase CurrentPhase { get; set; }
        public Guid HostUserId { get; set; }
        public int PlayersCount { get; set; }
    }
}
