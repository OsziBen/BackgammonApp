using MediatR;

namespace Application.GameSessions.Commands.PlayerTimeoutExpired
{
    public record PlayerTimeoutExpiredCommand(Guid GamePlayerId) : IRequest<Unit>;
}
