using Application.Interfaces.Repository;
using Application.Interfaces.Repository.Group;
using Application.Interfaces.Repository.GroupJoinRequest;
using Application.Interfaces.Repository.GroupMembership;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Enums.Group;
using Common.Exceptions;
using Domain.Group;
using MediatR;

namespace Application.Groups.Commands.JoinGroup
{
    public class JoinGroupCommandHandler : IRequestHandler<JoinGroupCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGroupReadRepository _groupReadRepository;
        private readonly IGroupMembershipReadRepository _groupMembershipReadRepository;
        private readonly IGroupJoinRequestReadRepository _groupJoinRequestReadRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public JoinGroupCommandHandler(
            IUnitOfWork uow,
            IGroupReadRepository groupReadRepository,
            IGroupMembershipReadRepository groupMembershipReadRepository,
            IGroupJoinRequestReadRepository groupJoinRequestReadRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _uow = uow;
            _groupReadRepository = groupReadRepository;
            _groupMembershipReadRepository = groupMembershipReadRepository;
            _groupJoinRequestReadRepository = groupJoinRequestReadRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(JoinGroupCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var group = await _groupReadRepository
                .GetByIdAsync(request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(Group), request.GroupId);

            if (group.Visibility == GroupVisibility.Private)
            {
                throw new BusinessRuleException(
                    FunctionCode.CannotJoinPrivateGroup,
                    "Cannot join a private group.");
            }

            if (await _groupMembershipReadRepository.ExistsAsync(request.UserId, group.Id, cancellationToken))
            {
                throw new BusinessRuleException(
                    FunctionCode.UserAlreadyActiveMember,
                    "User is already an active member of this group.");
            }

            if (await _groupJoinRequestReadRepository.HasPendingRequestAsync(request.UserId, group.Id, cancellationToken))
            {
                throw new BusinessRuleException(
                    FunctionCode.JoinAlreadyRequested,
                    "Join already requested.");
            }

            if (group.GroupMemberships.Count >= group.MaxMembers)
            {
                throw new BusinessRuleException(
                    FunctionCode.GroupReachedMaxMembersLimit,
                    "Group reached max members limit.");
            }

            await _uow.GroupJoinRequestsWrite.AddAsync(new Domain.GroupJoinRequest.GroupJoinRequest
            {
                UserId = request.UserId,
                GroupId = request.GroupId,
                Status = JoinRequestStatus.Pending,
                CreatedAt = now,
            }, cancellationToken);

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
