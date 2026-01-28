using MediatR;

namespace Application.GameSessions.Commands.DeclineDoublingCube
{
    public record DeclineDoublingCubeCommand(Guid SessionId, Guid PlayerId) : IRequest<Unit>;
}
