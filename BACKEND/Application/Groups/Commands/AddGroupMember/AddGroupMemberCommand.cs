using MediatR;

namespace Application.Groups.Commands.AddGroupMember
{
    public record AddGroupMemberCommand(Guid GroupId, string UserName, Guid UserId) : IRequest<Unit>;
}
