using Application.Groups.Responses;
using MediatR;

namespace Application.Groups.Commands.ListGroupMembers
{
    public record ListGroupMembersCommand(Guid GroupId) : IRequest<GroupMembersResponse>;
}
