using MediatR;

namespace Application.GameSessions.Commands.OfferDoublingCube
{
    public record OfferDoublingCubeCommand(Guid SessionId, Guid PlayerId) : IRequest<Unit>;
}
