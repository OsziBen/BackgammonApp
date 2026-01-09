using MediatR;

namespace Application.GameSessions.Commands.PlayerDisconnected
{
    public record PlayerDisconnectedCommand(Guid GamePlayerId) : IRequest<Unit>;
}
