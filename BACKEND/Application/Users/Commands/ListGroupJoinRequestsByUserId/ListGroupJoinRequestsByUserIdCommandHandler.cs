using Application.Interfaces.Repository.GroupJoinRequest;
using Application.Users.Responses;
using MediatR;

namespace Application.Users.Commands.ListGroupJoinRequestsByUserId
{
    public class ListGroupJoinRequestsByUserIdCommandHandler : IRequestHandler<ListGroupJoinRequestsByUserIdCommand, List<UserGroupJoinRequestResponse>>
    {
        private readonly IGroupJoinRequestReadRepository _groupJoinRequestReadRepository;

        public ListGroupJoinRequestsByUserIdCommandHandler(IGroupJoinRequestReadRepository groupJoinRequestReadRepository)
        {
            _groupJoinRequestReadRepository = groupJoinRequestReadRepository;
        }

        public async Task<List<UserGroupJoinRequestResponse>> Handle(ListGroupJoinRequestsByUserIdCommand request, CancellationToken cancellationToken)
        {
            var joinRequests = await _groupJoinRequestReadRepository.GetAllByUserIdAsync(request.UserId, cancellationToken);

            return joinRequests.Select(joinRequest => new UserGroupJoinRequestResponse
            {
                Id = joinRequest.Id,
                GroupName = joinRequest.Group.Name,
                Status = joinRequest.Status.ToString(),
                CreatedAt = joinRequest.CreatedAt,
            }).ToList();
        }
    }
}
