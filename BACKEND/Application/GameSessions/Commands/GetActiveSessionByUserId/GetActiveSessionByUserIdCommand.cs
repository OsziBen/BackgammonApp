using Application.GameSessions.Responses;
using MediatR;

namespace Application.GameSessions.Commands.GetActiveSessionByUserId
{
    public record GetActiveSessionByUserIdCommand(Guid UserId) : IRequest<GetActiveSessionByUserIdResponse?>;
}
