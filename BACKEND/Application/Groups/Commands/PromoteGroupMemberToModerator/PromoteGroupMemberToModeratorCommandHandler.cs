using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GroupMembership;
using Application.Interfaces.Repository.GroupRole;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Exceptions;
using Domain.GroupMembership;
using Domain.GroupMembershipRole;
using Domain.GroupRole;
using MediatR;

namespace Application.Groups.Commands.PromoteGroupMemberToModerator
{
    public class PromoteGroupMemberToModeratorCommandHandler : IRequestHandler<PromoteGroupMemberToModeratorCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IGroupMembershipReadRepository _membershipRead;
        private readonly IGroupRoleReadRepository _roleRead;

        public PromoteGroupMemberToModeratorCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider,
            IGroupMembershipReadRepository membershipRead,
            IGroupRoleReadRepository roleRead)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
            _membershipRead = membershipRead;
            _roleRead = roleRead;
        }

        public async Task<Unit> Handle(PromoteGroupMemberToModeratorCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var membership = await _membershipRead
                .GetAsync(request.TargetUserId, request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(GroupMembership), request.TargetUserId);

            var moderatorRole = await _roleRead
                .GetBySystemNameAsync(GroupRoleConstants.Moderator, cancellationToken)
                .GetOrThrowAsync(nameof(GroupRole), GroupRoleConstants.Moderator);

            var memeberRole = await _roleRead
                .GetBySystemNameAsync(GroupRoleConstants.Member, cancellationToken)
                .GetOrThrowAsync(nameof(GroupRole), GroupRoleConstants.Member);

            var activeRoles = await _uow.GroupMembershipRolesWrite
                .GetActiveRolesAsync(membership.Id, cancellationToken);

            var isAlreadyModerator = activeRoles.Any(x => x.GroupRoleId == moderatorRole.Id);

            if (isAlreadyModerator)
            {
                throw new BusinessRuleException(
                    FunctionCode.UserIsAlreadyModerator,
                    "Selected user is already a moderator.");
            }

            var currentModerators = await _uow.GroupMembershipRolesWrite
                .CountActiveByRoleAsync(membership.GroupId, GroupRoleConstants.Moderator, cancellationToken);

            if (currentModerators >= membership.Group.MaxModerators)
            {
                throw new BusinessRuleException(
                    FunctionCode.ModeratorLimitReached,
                    "Moderator limit reached.");
            }

            foreach (var role in activeRoles)
            {
                role.IsActive = false;
                role.RevokedAt = now;
            }

            await _uow.GroupMembershipRolesWrite.AddAsync(
                new GroupMembershipRole
                {
                    GroupMembershipId = membership.Id,
                    GroupRoleId = moderatorRole.Id,
                    IsActive = true,
                    AssignedAt = now,
                    GrantedBy = request.CurrentUserId
                },
                cancellationToken);

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
