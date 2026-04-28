using MediatR;

namespace Application.Groups.Commands.AddGroupMember
{
    public record AddGroupMemberCommand(Guid GroupId, string UserName, Guid CurrentUserId) : IRequest<Unit>;
}
