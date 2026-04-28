using Application.Groups.Responses;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.Group;
using Application.Interfaces.Repository.GroupRole;
using Application.Interfaces.Repository.User;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Enums.Group;
using Common.Exceptions;
using Domain.GameSession;
using Domain.Group;
using Domain.GroupMembership;
using Domain.GroupMembershipRole;
using Domain.GroupRole;
using Domain.User;
using MediatR;

namespace Application.Groups.Commands.CreateGroup
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, BaseGroupResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly IGroupReadRepository _groupReadRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IGroupRoleReadRepository _groupRoleReadRepository;
        private readonly IUserReadRepository _userReadRepository;

        public CreateGroupCommandHandler(
            IUnitOfWork uow,
            IGroupReadRepository groupReadRepository,
            IDateTimeProvider dateTimeProvider,
            IGroupRoleReadRepository groupRoleReadRepository,
            IUserReadRepository userReadRepository)
        {
            _uow = uow;
            _groupReadRepository = groupReadRepository;
            _dateTimeProvider = dateTimeProvider;
            _groupRoleReadRepository = groupRoleReadRepository;
            _userReadRepository = userReadRepository;
        }

        public async Task<BaseGroupResponse> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;
            var normalizedName = request.Name.Trim();

            var user = await _userReadRepository
                .GetByIdAsync(request.UserId, cancellationToken)
                .GetOrThrowAsync(nameof(User), request.UserId);

            var hasGroupWithSameName = await _groupReadRepository.ExistsByNameAsync(normalizedName, cancellationToken);

            if (hasGroupWithSameName)
            {
                throw new BusinessRuleException(
                    FunctionCode.GroupWithGroupNameAlreadyExists,
                    $"Group with name {normalizedName} already exists.");
            }

            var (maxMembers, maxModerators) = request.SizePreset switch
            {
                GroupSizePreset.Small => (10, 0),
                GroupSizePreset.Medium => (30, 2),
                GroupSizePreset.Large => (100, 5),
                _ => (30, 2)
            };

            var group = new Group
            {
                CreatorId = request.UserId,
                Name = normalizedName,
                Description = request.Description.Trim(),
                Visibility = request.Visibility,
                SizePreset = request.SizePreset,
                JoinPolicy = request.Visibility == GroupVisibility.Public
                    ? GroupJoinPolicy.Request
                    : GroupJoinPolicy.InviteOnly,
                MaxMembers = maxMembers,
                MaxModerators = maxModerators,

                CreatedAt = now,
                LastUpdatedAt = now,
                IsDeleted = false
            };

            var membership = new GroupMembership
            {
                GroupId = group.Id,
                UserId = group.CreatorId,
                JoinedAt = now,
                IsActive = true
            };

            var ownerRole = await _groupRoleReadRepository
                .GetBySystemNameAsync(GroupRoleConstants.Owner, cancellationToken)
                .GetOrThrowAsync(nameof(GroupRole), GroupRoleConstants.Owner);

            var membershipRole = new GroupMembershipRole
            {
                GroupMembershipId = membership.Id,
                GroupRoleId = ownerRole.Id,
                IsActive = true,
                AssignedAt = now,
                GrantedBy = request.UserId
            };

            membership.GroupRoles.Add(membershipRole);
            group.GroupMemberships.Add(membership);

            await _uow.GroupsWrite.AddAsync(group, cancellationToken);
            
            await _uow.CommitAsync(cancellationToken);

            return new BaseGroupResponse
            {
                Id = group.Id,
                CreatorName = user.UserName,
                Name = group.Name,
                Description = group.Description,
                Visibility = group.Visibility.ToString(),
                JoinPolicy = group.JoinPolicy.ToString(),
                SizePreset = group.SizePreset.ToString(),
                MaxMembers = group.MaxMembers,
                MaxModerators = group.MaxModerators,
                CreatedAt = group.CreatedAt,
            };
        }
    }
}
