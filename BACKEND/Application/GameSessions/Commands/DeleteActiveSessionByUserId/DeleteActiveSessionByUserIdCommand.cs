using MediatR;

namespace Application.GameSessions.Commands.DeleteActiveSessionByUserId
{
    public record DeleteActiveSessionByUserIdCommand(Guid UserId ,Guid SessionId) : IRequest<Unit>;
}
