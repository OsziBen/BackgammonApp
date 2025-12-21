using MediatR;

namespace Application.GameSessions.Commands.CreateGameSession
{
    public record CreateGameSessionCommand(
        Guid MatchId,
        string SessionCode
    ) : IRequest<Guid>;
}
