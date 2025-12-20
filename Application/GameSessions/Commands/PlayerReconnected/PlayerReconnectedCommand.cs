using MediatR;

namespace Application.GameSessions.Commands.PlayerReconnected
{
    public record PlayerReconnectedCommand(Guid GamePlayerId) : IRequest;
}
