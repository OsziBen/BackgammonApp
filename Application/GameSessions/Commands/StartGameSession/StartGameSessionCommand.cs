using MediatR;

namespace Application.GameSessions.Commands.StartGameSession
{
    public record StartGameSessionCommand(Guid GameSessionId) : IRequest
    {
    }
}
