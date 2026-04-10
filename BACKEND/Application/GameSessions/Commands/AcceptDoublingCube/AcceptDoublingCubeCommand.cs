using MediatR;

namespace Application.GameSessions.Commands.AcceptDoublingCube
{
    public record AcceptDoublingCubeCommand(Guid SessionId, Guid UserId) : IRequest<Unit>;
}
