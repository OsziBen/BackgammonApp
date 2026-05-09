using Application.Interfaces.Repository;
using Application.Interfaces.Repository.Group;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Exceptions;
using Domain.Group;
using Domain.GroupMembership;
using MediatR;

namespace Application.Groups.Commands.RemoveGroupMember
{
    public class RemoveGroupMemberCommandHandler : IRequestHandler<RemoveGroupMemberCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IGroupReadRepository _groupReadRepository;

        public RemoveGroupMemberCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider,
            IGroupReadRepository groupReadRepository)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
            _groupReadRepository = groupReadRepository;
        }

        public async Task<Unit> Handle(RemoveGroupMemberCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var membership = await _uow.GroupMembershipsWrite
                .GetAsync(request.UserId, request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(GroupMembership), request.GroupId);

            var group = await _groupReadRepository
                .GetByIdAsync(request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(Group), request.GroupId);

            if (request.UserId == group.CreatorId)
            {
                throw new BusinessRuleException(
                    FunctionCode.CannotRemoveGroupOwner,
                    "Cannot remove the owner of the group.");
            }

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
