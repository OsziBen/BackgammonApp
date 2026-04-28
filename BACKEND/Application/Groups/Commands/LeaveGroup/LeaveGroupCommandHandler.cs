using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Domain.GroupMembership;
using MediatR;

namespace Application.Groups.Commands.LeaveGroup
{
    public class LeaveGroupCommandHandler : IRequestHandler<LeaveGroupCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;

        public LeaveGroupCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(LeaveGroupCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var membership = await _uow.GroupMembershipsWrite
                .GetAsync(request.UserId, request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(GroupMembership), request.GroupId);

            membership.IsActive = false;
            membership.DisabledAt = now;

            var roles = await _uow.GroupMembershipRolesWrite
                .GetActiveRolesAsync(membership.Id, cancellationToken);

            foreach (var role in roles)
            {
                role.IsActive = false;
                role.RevokedAt = now;
            }

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
