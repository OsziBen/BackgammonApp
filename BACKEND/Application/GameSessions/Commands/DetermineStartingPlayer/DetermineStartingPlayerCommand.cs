using MediatR;

namespace Application.GameSessions.Commands.DetermineStartingPlayer
{
    public record DetermineStartingPlayerCommand(Guid SessionId) : IRequest<Unit>;
}
