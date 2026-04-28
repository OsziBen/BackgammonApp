using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GroupMembership;
using Application.Interfaces.Repository.GroupRole;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Exceptions;
using Domain.GroupMembership;
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

            var roles = await _uow.GroupMembershipRolesWrite
                .GetActiveRolesAsync(membership.Id, cancellationToken);

            var moderator = roles.FirstOrDefault(r => r.GroupRoleId == moderatorRole.Id);

            if (moderator == null)
            {
                throw new BusinessRuleException(
                    FunctionCode.UserIsNotModerator,
                    "Selected user is not a moderator.");
            }
                

            var isOwner = await _uow.GroupMembershipRolesWrite
                .IsOwnerAsync(membership.Id, cancellationToken);

            if (isOwner)
            {
                throw new BusinessRuleException(
                    FunctionCode.CannotDemoteGroupOwner,
                    "Cannot demote group owner.");
            }

            moderator.IsActive = false;
            moderator.RevokedAt = now;

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
