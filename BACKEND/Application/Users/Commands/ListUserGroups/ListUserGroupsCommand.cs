using Application.Groups.Responses;
using MediatR;

namespace Application.Users.Commands.ListUserGroups
{
    public record ListUserGroupsCommand(Guid UserId) : IRequest<List<GroupBaseResponse>>;
}
