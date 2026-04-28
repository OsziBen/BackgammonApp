using MediatR;

namespace Application.Groups.Commands.JoinGroup
{
    public record JoinGroupCommand(Guid GroupId, Guid UserId) : IRequest<Unit>;
}
