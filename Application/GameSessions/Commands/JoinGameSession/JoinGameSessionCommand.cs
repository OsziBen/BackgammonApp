using Application.GameSessions.Responses;
using MediatR;

namespace Application.GameSessions.Commands.JoinGameSession
{
    public record JoinGameSessionCommand(
        string SessionCode,
        Guid UserId,
        string ConnectionId
        ) : IRequest<GameSessionSnapshotResponse>;
}
