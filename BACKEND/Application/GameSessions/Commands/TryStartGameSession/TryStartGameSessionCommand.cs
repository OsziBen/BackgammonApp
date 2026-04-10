using MediatR;

namespace Application.GameSessions.Commands.TryStartGameSession
{
    public record TryStartGameSessionCommand(Guid SessionId) : IRequest<Unit>;
}
