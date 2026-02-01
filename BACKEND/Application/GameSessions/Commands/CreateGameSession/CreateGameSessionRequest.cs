using Domain.GameSession;

namespace Application.GameSessions.Commands.CreateGameSession
{
    public class CreateGameSessionRequest
    {
        public Guid HostPlayerId { get; set; }

        public GameSessionSettings Settings { get; set; } = null!;
    }
}
