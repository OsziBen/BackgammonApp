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

namespace Application.Groups.Commands.PromoteToModerator
{
    public class PromoteToModeratorCommandHandler : IRequestHandler<PromoteToModeratorCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IGroupMembershipReadRepository _membershipRead;
        private readonly IGroupRoleReadRepository _roleRead;

        public PromoteToModeratorCommandHandler(
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

        public async Task<Unit> Handle(PromoteToModeratorCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var membership = await _membershipRead
                .GetAsync(request.TargetUserId, request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(GroupMembership), request.TargetUserId);

            var group = membership.Group;

            var currentModerators = await _uow.GroupMembershipRolesWrite
                .CountActiveByRoleAsync(group.Id, GroupRoleConstants.Moderator, cancellationToken);

            if (currentModerators >= group.MaxModerators)
            {
                throw new BusinessRuleException(
                    FunctionCode.ModeratorLimitReached,
                    "Moderator limit reached.");
            }

            var moderatorRole = await _roleRead
                .GetBySystemNameAsync(GroupRoleConstants.Moderator, cancellationToken)
                .GetOrThrowAsync(nameof(GroupRole), GroupRoleConstants.Moderator);

            var alreadyModerator = await _uow.GroupMembershipRolesWrite
                .ExistsActiveRoleAsync(membership.Id, moderatorRole.Id, cancellationToken);

            if (alreadyModerator)
            {
                throw new BusinessRuleException(
                    FunctionCode.UserIsAlreadyModerator,
                    "Selected user is already a moderator.");
            }

            await _uow.GroupMembershipRolesWrite.AddAsync(new GroupMembershipRole
            {
                GroupMembershipId = membership.Id,
                GroupRoleId = moderatorRole.Id,
                IsActive = true,
                AssignedAt = now,
                GrantedBy = request.CurrentUserId
            }, cancellationToken);

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
