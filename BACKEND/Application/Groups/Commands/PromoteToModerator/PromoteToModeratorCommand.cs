using MediatR;

namespace Application.Groups.Commands.PromoteToModerator
{
    public record PromoteToModeratorCommand(
        Guid GroupId,
        Guid TargetUserId,
        Guid CurrentUserId) : IRequest<Unit>;
}
