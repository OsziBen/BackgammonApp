using Application.Groups.Helpers;
using Application.Groups.Responses;
using Application.Interfaces.Repository.Group;
using Application.Interfaces.Repository.GroupMembership;
using MediatR;

namespace Application.Users.Commands.ListUserGroups
{
    public class ListUserGroupsCommandHandler : IRequestHandler<ListUserGroupsCommand, List<GroupBaseResponse>>
    {
        private readonly IGroupReadRepository _groupReadRepository;
        private readonly IGroupMembershipReadRepository _groupMembershipReadRepository;

        public ListUserGroupsCommandHandler(
            IGroupReadRepository groupReadRepository,
            IGroupMembershipReadRepository groupMembershipReadRepository)
        {
            _groupReadRepository = groupReadRepository;
            _groupMembershipReadRepository = groupMembershipReadRepository;
        }

        public async Task<List<GroupBaseResponse>> Handle(ListUserGroupsCommand request, CancellationToken cancellationToken)
        {
            var groups = await _groupReadRepository
                .GetAllByUserIdAsync(request.UserId, cancellationToken);

            var memberships = await _groupMembershipReadRepository
                .GetMembershipsWithRolesByUserIdAsync(
                    request.UserId,
                    cancellationToken);

            var membershipLookup = memberships.ToDictionary(
                 m => m.GroupId,
                 m => m);

            return groups.Select(group =>
            {
                membershipLookup.TryGetValue(group.Id, out var membership);

                return GroupResponseMapper.ToBaseResponse(
                    group,
                    membership,
                    hasPendingRequest: false);
            }).ToList();
        }
    }
}
