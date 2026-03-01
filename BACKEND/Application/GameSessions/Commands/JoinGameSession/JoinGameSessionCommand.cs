using Domain.GameSession.Results;
using MediatR;

namespace Application.GameSessions.Commands.JoinGameSession
{
    public record JoinGameSessionCommand(
        string SessionCode,
        Guid UserId
        ) : IRequest<JoinResult>;
}
