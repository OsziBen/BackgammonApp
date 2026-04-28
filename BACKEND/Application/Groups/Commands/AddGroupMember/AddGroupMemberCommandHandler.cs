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

        public async Task<Unit> Handle(AddGroupMemberCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var group = await _groupReadRepository
                .GetByIdAsync(request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(Group), request.GroupId);

            if (group.Visibility != GroupVisibility.Private)
            {
                throw new BusinessRuleException(
                    FunctionCode.CannotAddUserDIrectlyInPublicGroup,
                    "Direct add is only allowed for private groups.");
            }

            var user = await _userReadRepository
                .GetByUserNameAsync(request.UserName, cancellationToken)
                .GetOrThrowAsync(nameof(User), request.UserName);

            if (await _groupMembershipReadRepository.ExistsAsync(user.Id, group.Id, cancellationToken))
            {
                throw new BusinessRuleException(
                    FunctionCode.UserAlreadyActiveMember,
                    "User already a member.");
            }

            if (group.GroupMemberships.Count >= group.MaxMembers)
            {
                throw new BusinessRuleException(
                    FunctionCode.GroupReachedMaxMembersLimit,
                    "Group has reached its limit of members.");
            }

            var membership = await MembershipHelper.GetOrCreateMembershipAsync(
                _uow,
                user.Id,
                group.Id,
                now,
                cancellationToken);

            var role = await _groupRoleReadRepository
                .GetBySystemNameAsync(GroupRoleConstants.Member, cancellationToken)
                .GetOrThrowAsync(nameof(GroupRole), GroupRoleConstants.Member.ToString());

            await _uow.GroupMembershipRolesWrite.AddAsync(new GroupMembershipRole
            {
                GroupMembershipId = membership.Id,
                GroupRoleId = role.Id,
                IsActive = true,
                AssignedAt = now,
                GrantedBy = request.CurrentUserId
            }, cancellationToken);

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
