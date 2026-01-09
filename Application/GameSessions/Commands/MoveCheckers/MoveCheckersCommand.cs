using Application.GameSessions.Requests;
using MediatR;

namespace Application.GameSessions.Commands.MoveCheckers
{
    public record MoveCheckersCommand(
        Guid SessionId,
        Guid PlayerId,
        IReadOnlyList<MoveDto> Moves
        ) : IRequest<Unit>;
}
