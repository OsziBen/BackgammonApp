using Application.Groups.Responses;
using MediatR;

namespace Application.Groups.Commands.GetAllPublicGroups
{
    public record GetAllPublicGroupsCommand(Guid UserId) : IRequest<List<GroupBaseResponse>>;
}
