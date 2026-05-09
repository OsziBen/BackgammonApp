using MediatR;

namespace Application.Groups.Commands.PromoteGroupMemberToModerator
{
    public record PromoteGroupMemberToModeratorCommand(
        Guid GroupId,
        Guid TargetUserId,
        Guid CurrentUserId) : IRequest<Unit>;
}
