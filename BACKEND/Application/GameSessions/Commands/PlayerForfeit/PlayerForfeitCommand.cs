using MediatR;

namespace Application.GameSessions.Commands.PlayerForfeit
{
    public record PlayerForfeitCommand(Guid SessionId, Guid UserId) : IRequest<Unit>;
}
