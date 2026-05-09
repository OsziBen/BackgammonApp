using MediatR;

namespace Application.Groups.Commands.DemoteModerator
{
    public record DemoteModeratorCommand(
        Guid GroupId,
        Guid TargetUserId) : IRequest<Unit>;
}
