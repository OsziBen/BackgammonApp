using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.CreateGameSession
{
    public record CreateGameSessionCommand(Guid HostPlayerId, GameSessionSettings Settings) : IRequest<Guid>;
}
