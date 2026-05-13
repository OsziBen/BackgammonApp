using Application.Groups.Helpers;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.Group;
using Application.Interfaces.Repository.GroupMembership;
using Application.Interfaces.Repository.GroupRole;
using Application.Interfaces.Repository.User;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Enums.Group;
using Common.Exceptions;
using Domain.Group;
using Domain.GroupMembershipRole;
using Domain.GroupRole;
using Domain.User;
using MediatR;

namespace Application.Groups.Commands.AddGroupMember
{
    public class AddGroupMemberCommandHandler : IRequestHandler<AddGroupMemberCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IGroupReadRepository _groupReadRepository;
        private readonly IGroupMembershipReadRepository _groupMembershipReadRepository;
        private readonly IGroupRoleReadRepository _groupRoleReadRepository;
        private readonly IUserReadRepository _userReadRepository;

        public AddGroupMemberCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider,
            IGroupReadRepository groupReadRepository,
            IGroupMembershipReadRepository groupMembershipReadRepository,
            IGroupRoleReadRepository groupRoleReadRepository,
            IUserReadRepository userReadRepository)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
            _groupReadRepository = groupReadRepository;
            _groupMembershipReadRepository = groupMembershipReadRepository;
            _groupRoleReadRepository = groupRoleReadRepository;
            _userReadRepository = userReadRepository;
        }

        public async Task<Unit> Handle(
            AddGroupMemberCommand request,
            CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var group = await _groupReadRepository
                .GetByIdAsync(request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(Group), request.GroupId);

            if (group.Visibility != GroupVisibility.Private)
            {
                throw new BusinessRuleException(
                    FunctionCode.CannotAddUserDirectlyInPublicGroup,
                    "Direct add is only allowed for private groups.");
            }

            if (group.GroupMemberships.Count(gm => gm.IsActive) >= group.MaxMembers)
            {
                throw new BusinessRuleException(
                    FunctionCode.GroupReachedMaxMembersLimit,
                    "Group has reached its limit of members.");
            }

            var user = await _userReadRepository
                .GetByUserNameAsync(request.UserName, cancellationToken)
                .GetOrThrowAsync(nameof(User), request.UserName);

            var existingMembership = await _uow.GroupMembershipsWrite
                .GetAnyAsync(user.Id, group.Id, cancellationToken);

            if (existingMembership?.IsActive == true)
            {
                throw new BusinessRuleException(
                    FunctionCode.UserAlreadyActiveMember,
                    "User already a member.");
            }

            var membership = await MembershipHelper.GetOrCreateMembershipAsync(
                _uow,
                user.Id,
                group.Id,
                now,
                cancellationToken);

            membership.IsActive = true;
            membership.DisabledAt = null;

            var memberRole = await _groupRoleReadRepository
                .GetBySystemNameAsync(GroupRoleConstants.Member, cancellationToken)
                .GetOrThrowAsync(nameof(GroupRole), GroupRoleConstants.Member);

            var activeRoles = await _uow.GroupMembershipRolesWrite
                .GetActiveRolesAsync(membership.Id, cancellationToken);

            foreach (var role in activeRoles)
            {
                role.IsActive = false;
                role.RevokedAt = now;
            }

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

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}