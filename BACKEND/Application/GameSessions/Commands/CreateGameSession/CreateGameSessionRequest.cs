using Domain.GameSession;

namespace Application.GameSessions.Commands.CreateGameSession
{
    public class CreateGameSessionRequest
    {
        public GameSessionSettings Settings { get; set; } = null!;
    }
}
