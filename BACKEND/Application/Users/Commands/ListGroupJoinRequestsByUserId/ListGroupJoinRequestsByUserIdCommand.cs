using Application.Users.Responses;
using MediatR;

namespace Application.Users.Commands.ListGroupJoinRequestsByUserId
{
    public record ListGroupJoinRequestsByUserIdCommand(Guid UserId) : IRequest<List<UserGroupJoinRequestResponse>>;
}
