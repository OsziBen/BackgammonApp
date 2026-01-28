using MediatR;

namespace Application.GameSessions.Commands.AcceptDoublingCube
{
    public record AcceptDoublingCubeCommand(Guid SessionId, Guid PlayerId) : IRequest<Unit>;
}
