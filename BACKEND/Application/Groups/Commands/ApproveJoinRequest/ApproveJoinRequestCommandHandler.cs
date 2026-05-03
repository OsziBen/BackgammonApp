using Application.Groups.Helpers;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GroupMembership;
using Application.Interfaces.Repository.GroupRole;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Enums.Group;
using Common.Exceptions;
using Domain.GroupJoinRequest;
using Domain.GroupMembershipRole;
using Domain.GroupRole;
using MediatR;

namespace Application.Groups.Commands.ApproveJoinRequest
{
    public class ApproveJoinRequestCommandHandler : IRequestHandler<ApproveJoinRequestCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IGroupMembershipReadRepository _groupMembershipReadRepository;
        private readonly IGroupRoleReadRepository _groupRoleReadRepository;

        public ApproveJoinRequestCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider,
            IGroupMembershipReadRepository groupMembershipReadRepository,
            IGroupRoleReadRepository groupRoleReadRepository)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
            _groupMembershipReadRepository = groupMembershipReadRepository;
            _groupRoleReadRepository = groupRoleReadRepository;
        }

        public async Task<Unit> Handle(ApproveJoinRequestCommand request, CancellationToken cancellationToken)
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
                    "Group IDs are not macthing.");
            }

            if (await _groupMembershipReadRepository.ExistsAsync(joinRequest.UserId, request.GroupId, cancellationToken))
            {
                throw new BusinessRuleException(
                    FunctionCode.UserAlreadyActiveMember,
                    "User is already an active member of the group.");
            }

            var membership = await MembershipHelper.GetOrCreateMembershipAsync(
                _uow,
                joinRequest.UserId,
                request.GroupId,
                now,
                cancellationToken);

            var role = await _groupRoleReadRepository
                .GetBySystemNameAsync(GroupRoleConstants.Member, cancellationToken)
                .GetOrThrowAsync(nameof(GroupRole), GroupRoleConstants.Member);

            await _uow.GroupMembershipRolesWrite.AddAsync(new GroupMembershipRole
            {
                GroupMembershipId = membership.Id,
                GroupRoleId = role.Id,
                IsActive = true,
                AssignedAt = now,
                GrantedBy = request.UserId
            }, cancellationToken);

            joinRequest.Status = JoinRequestStatus.Approved;
            joinRequest.ReviewedAt = now;
            joinRequest.ReviewedByUserId = request.UserId;

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
