using Application.Groups.Responses;
using Application.Interfaces.Repository.GroupJoinRequest;
using MediatR;

namespace Application.Groups.Commands.ListGroupJoinRequests
{
    public class ListGroupJoinRequestsCommandHandler : IRequestHandler<ListGroupJoinRequestsCommand, List<GroupJoinRequestResponse>>
    {
        private readonly IGroupJoinRequestReadRepository _groupJoinRequestReadRepository;


        public ListGroupJoinRequestsCommandHandler(IGroupJoinRequestReadRepository groupJoinRequestReadRepository)
        {
            _groupJoinRequestReadRepository = groupJoinRequestReadRepository;
        }

        public async Task<List<GroupJoinRequestResponse>> Handle(ListGroupJoinRequestsCommand request, CancellationToken cancellationToken)
        {
            var groupJoinRequests = await _groupJoinRequestReadRepository.GetAllByGroupIdAsync(request.GroupId, cancellationToken);

            return groupJoinRequests.Select(joinRequest => new GroupJoinRequestResponse
            {
                Id = joinRequest.Id,
                UserName = joinRequest.User.UserName,
                Status = joinRequest.Status.ToString(),
                CreatedAt = joinRequest.CreatedAt,
                ReviewedAt = joinRequest.ReviewedAt,
                ReviewedByUser = joinRequest.ReviewedByUser?.UserName
            }).ToList();
        }
    }
}
