using Application.Groups.Responses;
using MediatR;

namespace Application.Groups.Commands.GetAllGroups
{
    public record GetAllPublicGroupsCommand() : IRequest<List<BaseGroupResponse>>;
}
