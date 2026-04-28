using MediatR;

namespace Application.Groups.Commands.ApproveJoinRequest
{
    public record ApproveJoinRequestCommand(Guid GroupId, Guid UserId, Guid RequestId) : IRequest<Unit>;
}
