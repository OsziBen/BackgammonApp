using MediatR;

namespace Application.Groups.Commands.LeaveGroup
{
    public record LeaveGroupCommand(Guid GroupId, Guid UserId) : IRequest<Unit>;
}
