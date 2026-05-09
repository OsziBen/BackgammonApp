using Application.Groups.Responses;
using Application.Interfaces.Repository.Group;
using Application.Interfaces.Repository.GroupMembership;
using Application.Shared;
using Application.Users.Responses;
using MediatR;
using System.Text.RegularExpressions;

namespace Application.Groups.Commands.ListGroupMembers
{
    public class ListGroupMembersCommandHandler : IRequestHandler<ListGroupMembersCommand, GroupMembersResponse>
    {
        private readonly IGroupReadRepository _groupReadRepository;
        private readonly IGroupMembershipReadRepository _groupMembershipReadRepository;

        public ListGroupMembersCommandHandler(
            IGroupReadRepository groupReadRepository,
            IGroupMembershipReadRepository groupMembershipReadRepository)
        {
            _groupReadRepository = groupReadRepository;
            _groupMembershipReadRepository = groupMembershipReadRepository;
        }

        public async Task<GroupMembersResponse> Handle(ListGroupMembersCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupReadRepository
                .GetByIdAsync(request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(Group), request.GroupId);

            var memberships = await _groupMembershipReadRepository
                .GetUsersByGroupIdAsync(request.GroupId, cancellationToken);

            List<UserBaseResponse> members = [];

            foreach (var membership in memberships)
            {
                var groupUser = membership.User;
                var groupMembershipRole = membership.GroupRoles
                    .FirstOrDefault(r => r.IsActive);

                members.Add(new UserBaseResponse
                {
                    Id = groupUser.Id,
                    UserName = groupUser.UserName,
                    JoinedAt = membership.JoinedAt,
                    Rating = groupUser.Rating,
                    ExperiencePoints = groupUser.ExperiencePoints,

                    GroupRoleName = groupMembershipRole?.GroupRole?.Name,
                    AssignedAt = groupMembershipRole?.AssignedAt,
                    GrantedByUserName = groupMembershipRole?.GrantedByUser?.UserName
                });
            }

            return new GroupMembersResponse
            {
                Members = members,
                MaxModeratorNumber = group.MaxModerators,
                CurrentModeratorNumber = members.Select(m => m.GroupRoleName == "Moderator").Count()
            };
        }
    }
}
