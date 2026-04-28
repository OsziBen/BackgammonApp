using Application.Groups.Responses;
using MediatR;

namespace Application.Groups.Commands.ListMembers
{
    public record ListMembersCommand(Guid GroupId) : IRequest<GroupMembersResponse>;
}
