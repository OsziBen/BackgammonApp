using MediatR;

namespace Application.GameSessions.Commands.DeleteActiveSessionByUserId
{
    public record DeleteActiveSessionByUserIdCommand(Guid SessionId) : IRequest<Unit>;
}
