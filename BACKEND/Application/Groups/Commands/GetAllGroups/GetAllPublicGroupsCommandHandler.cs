using Application.Groups.Responses;
using Application.Interfaces.Repository.Group;
using Application.Interfaces.Repository.GroupJoinRequest;
using Application.Interfaces.Repository.GroupMembership;
using MediatR;

namespace Application.Groups.Commands.GetAllGroups
{
    public class GetAllPublicGroupsCommandHandler : IRequestHandler<GetAllPublicGroupsCommand, List<BaseGroupResponse>>
    {
        private readonly IGroupReadRepository _groupReadRepository;
        private readonly IGroupMembershipReadRepository _groupMembershipReadRepository;
        private readonly IGroupJoinRequestReadRepository _groupJoinRequestReadRepository;

        public GetAllPublicGroupsCommandHandler(
            IGroupReadRepository groupReadRepository,
            IGroupMembershipReadRepository groupMembershipReadRepository,
            IGroupJoinRequestReadRepository groupJoinRequestReadRepository)
        {
            _groupReadRepository = groupReadRepository;
            _groupMembershipReadRepository = groupMembershipReadRepository;
            _groupJoinRequestReadRepository = groupJoinRequestReadRepository;
        }

        public async Task<List<BaseGroupResponse>> Handle(GetAllPublicGroupsCommand request, CancellationToken cancellationToken)
        {
            var groups = await _groupReadRepository.GetAllPublicAsync(cancellationToken);

            var memberships = await _groupMembershipReadRepository.GetMembershipsByUserIdAsync(request.UserId, cancellationToken);
            var membershipGroupIds = memberships
                .Select(m => m.GroupId)
                .ToHashSet();

            var pendingJoinRequests = await _groupJoinRequestReadRepository.GetAllPendingByUserIdAsync(request.UserId, cancellationToken);
            var pendingJoinRequestGroupIds = pendingJoinRequests
                .Select(jr => jr.GroupId)
                .ToHashSet();

            return groups.Select(g => new BaseGroupResponse
            {
                Id = g.Id,
                CreatorName = g.Creator.UserName,
                Name = g.Name,
                Description = g.Description,
                Visibility = g.Visibility.ToString(),
                JoinPolicy = g.JoinPolicy.ToString(),
                SizePreset = g.SizePreset.ToString(),
                MaxMembers = g.MaxMembers,
                CanJoin = !membershipGroupIds.Contains(g.Id) && !pendingJoinRequestGroupIds.Contains(g.Id),
                CreatedAt = g.CreatedAt,
            }).ToList();
        }
    }
}
