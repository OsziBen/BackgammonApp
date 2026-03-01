using Application.GameSessions.Responses;
using Domain.GameSession;
using MediatR;

namespace Application.GameSessions.Commands.CreateGameSession
{
    public record CreateGameSessionCommand(Guid UserId, GameSessionSettings Settings) : IRequest<CreateGameSessionResponse>;
}
