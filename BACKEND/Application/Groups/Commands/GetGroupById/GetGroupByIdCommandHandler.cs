using Application.Groups.Responses;
using Application.Interfaces.Repository.Group;
using Application.Interfaces.Repository.GroupJoinRequest;
using Application.Interfaces.Repository.GroupMembership;
using Application.Shared;
using Domain.Group;
using MediatR;

namespace Application.Groups.Commands.GetGroupById
{
    public class GetGroupByIdCommandHandler : IRequestHandler<GetGroupByIdCommand, BaseGroupResponse>
    {
        private readonly IGroupReadRepository _groupReadRepository;
        private readonly IGroupMembershipReadRepository _groupMembershipReadRepository;
        private readonly IGroupJoinRequestReadRepository _groupJoinRequestReadRepository;

        public GetGroupByIdCommandHandler(
            IGroupReadRepository groupReadRepository,
            IGroupMembershipReadRepository groupMembershipReadRepository,
            IGroupJoinRequestReadRepository groupJoinRequestReadRepository)
        {
            _groupReadRepository = groupReadRepository;
            _groupMembershipReadRepository = groupMembershipReadRepository;
            _groupJoinRequestReadRepository = groupJoinRequestReadRepository;
        }

        public async Task<BaseGroupResponse> Handle(GetGroupByIdCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupReadRepository
                .GetByIdAsync(request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(Group), request.GroupId);

            var memberships = await _groupMembershipReadRepository.GetMembershipsByUserIdAsync(request.UserId, cancellationToken);
            var membershipGroupIds = memberships
                .Select(m => m.GroupId)
                .ToHashSet();

            var pendingJoinRequests = await _groupJoinRequestReadRepository.GetAllPendingByUserIdAsync(request.UserId, cancellationToken);
            var pendingJoinRequestGroupIds = pendingJoinRequests
                .Select(jr => jr.GroupId)
                .ToHashSet();

            return new BaseGroupResponse
            {
                Id = group.Id,
                CreatorName = group.Creator.UserName,
                Name = group.Name,
                Description = group.Description,
                Visibility = group.Visibility.ToString(),
                JoinPolicy = group.JoinPolicy.ToString(),
                SizePreset = group.SizePreset.ToString(),
                MaxMembers = group.MaxMembers,
                MaxModerators = group.MaxModerators,
                CanJoin = !membershipGroupIds.Contains(request.GroupId) && !pendingJoinRequestGroupIds.Contains(request.GroupId),
                CreatedAt = group.CreatedAt,
            };
        }
    }
}
