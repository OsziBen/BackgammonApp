using Application.Groups.Responses;
using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Enums.Group;
using Common.Exceptions;
using Domain.Group;
using MediatR;

namespace Application.Groups.Commands.EditGroup
{
    public class EditGroupCommandHandler : IRequestHandler<EditGroupCommand, GroupBaseResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;

        public EditGroupCommandHandler(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider)
        {
            _uow = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<GroupBaseResponse> Handle(EditGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _uow.GroupsWrite
                .GetByIdAsync(request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(Group), request.GroupId);

            if (request.SizePreset < group.SizePreset)
            {
                throw new BusinessRuleException(
                    FunctionCode.CannotDowngradeGroupSize,
                    $"Cannot downgrade group size from {group.SizePreset} to {request.SizePreset}.");
            }

            group.Name = request.Name.Trim();
            group.Description = request.Description.Trim();
            group.Visibility = request.Visibility;
            group.SizePreset = request.SizePreset;

            (group.MaxMembers, group.MaxModerators) = request.SizePreset switch
            {
                GroupSizePreset.Small => (10, 0),
                GroupSizePreset.Medium => (30, 2),
                GroupSizePreset.Large => (100, 5),
                _ => (30, 2)
            };

            group.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _uow.CommitAsync(cancellationToken);

            return new GroupBaseResponse
            {
                Id = group.Id,
                CreatorName = group.Creator.UserName,
                Name = group.Name,
                Description = group.Description,
                Visibility = group.Visibility.ToString(),
                JoinPolicy = group.JoinPolicy.ToString(),
                SizePreset = group.SizePreset.ToString(),
                MaxMembers = group.MaxMembers,
                MaxModerators = group.MaxModerators,
                CanJoin = false,
                CreatedAt = group.CreatedAt,
            };
        }
    }
}
