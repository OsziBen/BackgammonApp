using Application.Groups.Helpers;
using Application.Groups.Responses;
using Application.Interfaces.Repository.Group;
using Application.Interfaces.Repository.GroupJoinRequest;
using Application.Interfaces.Repository.GroupMembership;
using MediatR;

namespace Application.Groups.Commands.GetAllPublicGroups
{
    public class GetAllPublicGroupsCommandHandler : IRequestHandler<GetAllPublicGroupsCommand, List<GroupBaseResponse>>
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

        public async Task<List<GroupBaseResponse>> Handle(GetAllPublicGroupsCommand request, CancellationToken cancellationToken)
        {
            var groups = await _groupReadRepository
                .GetAllPublicAsync(cancellationToken);

            var memberships = await _groupMembershipReadRepository
                .GetMembershipsWithRolesByUserIdAsync(
                    request.UserId,
                    cancellationToken);

            var membershipLookup = memberships.ToDictionary(
                m => m.GroupId,
                m => m);

            var pendingJoinRequests = await _groupJoinRequestReadRepository
                .GetAllPendingByUserIdAsync(
                    request.UserId,
                    cancellationToken);

            var pendingGroupIds = pendingJoinRequests
                .Select(x => x.GroupId)
                .ToHashSet();

            return groups.Select(group =>
            {
                membershipLookup.TryGetValue(group.Id, out var membership);

                var hasPendingRequest = pendingGroupIds.Contains(group.Id);

                return GroupResponseMapper.ToBaseResponse(
                    group,
                    membership,
                    hasPendingRequest);
            }).ToList();
        }
    }
}
