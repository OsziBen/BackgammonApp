using MediatR;

namespace Application.Groups.Commands.RejectGroupJoinRequest
{
    public record RejectGroupJoinRequestCommand(Guid GroupId, Guid UserId, Guid RequestId) : IRequest<Unit>;
}
