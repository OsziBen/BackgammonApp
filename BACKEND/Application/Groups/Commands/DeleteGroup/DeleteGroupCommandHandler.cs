using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Domain.Group;
using MediatR;

namespace Application.Groups.Commands.DeleteGroup
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeleteGroupCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var group = await _uow.GroupsWrite
                .GetByIdAsync(request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(Group), request.GroupId);

            group.IsDeleted = true;
            group.DeletedAt = now;
            group.LastUpdatedAt = now;

            var memberships = await _uow.GroupMembershipsWrite
                .GetByGroupIdAsync(request.GroupId, cancellationToken);

            foreach (var membership in memberships)
            {
                membership.IsActive = false;
                membership.DisabledAt = now;

                var roles = membership.GroupRoles;

                foreach (var role in roles)
                {
                    role.IsActive = false;
                    role.RevokedAt = now;
                }
            }

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
