using Application.Groups.Helpers;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.Group;
using Application.Interfaces.Repository.GroupMembership;
using Application.Interfaces.Repository.GroupRole;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Enums.Group;
using Common.Exceptions;
using Domain.Group;
using Domain.GroupJoinRequest;
using Domain.GroupMembershipRole;
using Domain.GroupRole;
using MediatR;

namespace Application.Groups.Commands.ApproveGroupJoinRequest
{
    public class ApproveGroupJoinRequestCommandHandler
        : IRequestHandler<ApproveGroupJoinRequestCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IGroupMembershipReadRepository _groupMembershipReadRepository;
        private readonly IGroupRoleReadRepository _groupRoleReadRepository;
        private readonly IGroupReadRepository _groupReadRepository;

        public ApproveGroupJoinRequestCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider,
            IGroupMembershipReadRepository groupMembershipReadRepository,
            IGroupRoleReadRepository groupRoleReadRepository,
            IGroupReadRepository groupReadRepository)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
            _groupMembershipReadRepository = groupMembershipReadRepository;
            _groupRoleReadRepository = groupRoleReadRepository;
            _groupReadRepository = groupReadRepository;
        }

        public async Task<Unit> Handle(
            ApproveGroupJoinRequestCommand request,
            CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var joinRequest = await _uow.GroupJoinRequestsWrite
                .GetByIdAsync(request.RequestId, cancellationToken)
                .GetOrThrowAsync(nameof(GroupJoinRequest), request.RequestId);

            if (joinRequest.Status != JoinRequestStatus.Pending)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidJoinRequestStatus,
                    "Join request is not in pending state.");
            }

            if (joinRequest.GroupId != request.GroupId)
            {
                throw new BusinessRuleException(
                    FunctionCode.GroupMismatch,
                    "Group IDs are not matching.");
            }

            if (await _groupMembershipReadRepository.ExistsAsync(
                    joinRequest.UserId,
                    request.GroupId,
                    cancellationToken))
            {
                throw new BusinessRuleException(
                    FunctionCode.UserAlreadyActiveMember,
                    "User is already an active member of the group.");
            }

            var group = await _groupReadRepository
                .GetByIdAsync(joinRequest.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(Group), joinRequest.GroupId);

            if (group.GroupMemberships.Count(gm => gm.IsActive) >= group.MaxMembers)
            {
                throw new BusinessRuleException(
                    FunctionCode.GroupReachedMaxMembersLimit,
                    "Group has reached its limit of members.");
            }

            var membership = await MembershipHelper.GetOrCreateMembershipAsync(
                _uow,
                joinRequest.UserId,
                request.GroupId,
                now,
                cancellationToken);

            membership.IsActive = true;
            membership.DisabledAt = null;

            var activeRoles = await _uow.GroupMembershipRolesWrite
                .GetActiveRolesAsync(membership.Id, cancellationToken);

            foreach (var role in activeRoles)
            {
                role.IsActive = false;
                role.RevokedAt = now;
            }

            var memberRole = await _groupRoleReadRepository
                .GetBySystemNameAsync(GroupRoleConstants.Member, cancellationToken)
                .GetOrThrowAsync(nameof(GroupRole), GroupRoleConstants.Member);

            await _uow.GroupMembershipRolesWrite.AddAsync(
                new GroupMembershipRole
                {
                    GroupMembershipId = membership.Id,
                    GroupRoleId = memberRole.Id,
                    IsActive = true,
                    AssignedAt = now,
                    GrantedBy = request.UserId
                },
                cancellationToken);

            joinRequest.Status = JoinRequestStatus.Approved;
            joinRequest.ReviewedAt = now;
            joinRequest.ReviewedByUserId = request.UserId;

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}