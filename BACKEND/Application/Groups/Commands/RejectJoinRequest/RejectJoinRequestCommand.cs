using MediatR;

namespace Application.Groups.Commands.RejectJoinRequest
{
    public record RejectJoinRequestCommand(Guid GroupId, Guid UserId, Guid RequestId) : IRequest<Unit>;
}
