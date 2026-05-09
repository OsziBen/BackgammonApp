using MediatR;

namespace Application.Groups.Commands.ApproveGroupJoinRequest
{
    public record ApproveGroupJoinRequestCommand(Guid GroupId, Guid UserId, Guid RequestId) : IRequest<Unit>;
}
