using Application.Groups.Helpers;
using Application.Groups.Responses;
using Application.Interfaces.Repository.Group;
using Application.Interfaces.Repository.GroupJoinRequest;
using Application.Interfaces.Repository.GroupMembership;
using Application.Shared;
using Domain.Group;
using MediatR;

namespace Application.Groups.Commands.GetGroupById
{
    public class GetGroupByIdCommandHandler : IRequestHandler<GetGroupByIdCommand, GroupBaseResponse>
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

        public async Task<GroupBaseResponse> Handle(GetGroupByIdCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupReadRepository
                .GetByIdAsync(request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(Group), request.GroupId);

            var memberships = await _groupMembershipReadRepository
                .GetMembershipsWithRolesByUserIdAsync(
                    request.UserId,
                    cancellationToken);

            var membership = memberships
                .FirstOrDefault(m => m.GroupId == request.GroupId);

            var pendingJoinRequests = await _groupJoinRequestReadRepository
                .GetAllPendingByUserIdAsync(
                    request.UserId,
                    cancellationToken);

            var hasPendingRequest = pendingJoinRequests
                .Any(jr => jr.GroupId == request.GroupId);

            return GroupResponseMapper.ToBaseResponse(
                group,
                membership,
                hasPendingRequest);
        }
    }
}
