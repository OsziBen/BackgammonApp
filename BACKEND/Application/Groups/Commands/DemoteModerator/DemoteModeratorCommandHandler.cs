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

namespace Application.Groups.Commands.DemoteModerator
{
    public class DemoteModeratorCommandHandler : IRequestHandler<DemoteModeratorCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IGroupMembershipReadRepository _membershipRead;
        private readonly IGroupRoleReadRepository _roleRead;

        public DemoteModeratorCommandHandler(
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

        public async Task<Unit> Handle(DemoteModeratorCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var membership = await _membershipRead
                .GetAsync(request.TargetUserId, request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(GroupMembership), request.TargetUserId);

            var moderatorRole = await _roleRead
                .GetBySystemNameAsync(GroupRoleConstants.Moderator, cancellationToken)
                .GetOrThrowAsync(nameof(GroupRole), GroupRoleConstants.Moderator);

            var memberRole = await _roleRead
                .GetBySystemNameAsync(GroupRoleConstants.Member, cancellationToken)
                .GetOrThrowAsync(nameof(GroupRole), GroupRoleConstants.Member);

            var activeRoles = await _uow.GroupMembershipRolesWrite
                .GetActiveRolesAsync(membership.Id, cancellationToken);

            var activeModeratorRole = activeRoles
                .FirstOrDefault(x => x.GroupRoleId == moderatorRole.Id);

            if (activeModeratorRole == null)
            {
                throw new BusinessRuleException(
                    FunctionCode.UserIsNotModerator,
                    "Selected user is not a moderator.");
            }

            activeModeratorRole.IsActive = false;
            activeModeratorRole.RevokedAt = now;

            await _uow.GroupMembershipRolesWrite.AddAsync(
                new GroupMembershipRole
                {
                    GroupMembershipId = membership.Id,
                    GroupRoleId = memberRole.Id,
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
