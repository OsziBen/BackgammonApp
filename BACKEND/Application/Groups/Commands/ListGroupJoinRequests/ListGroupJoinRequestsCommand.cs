using Application.Groups.Responses;
using MediatR;

namespace Application.Groups.Commands.ListGroupJoinRequests
{
    public record ListGroupJoinRequestsCommand(Guid GroupId) : IRequest<List<GroupJoinRequestResponse>>;
}
