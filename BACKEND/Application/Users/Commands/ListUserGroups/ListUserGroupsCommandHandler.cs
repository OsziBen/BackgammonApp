using Application.Groups.Responses;
using Application.Interfaces.Repository.Group;
using MediatR;

namespace Application.Users.Commands.ListUserGroups
{
    public class ListUserGroupsCommandHandler : IRequestHandler<ListUserGroupsCommand, List<GroupBaseResponse>>
    {
        private readonly IGroupReadRepository _groupReadRepository;

        public ListUserGroupsCommandHandler(IGroupReadRepository groupReadRepository)
        {
            _groupReadRepository = groupReadRepository;
        }

        public async Task<List<GroupBaseResponse>> Handle(ListUserGroupsCommand request, CancellationToken cancellationToken)
        {
            var groups = await _groupReadRepository.GetAllByUserIdAsync(request.UserId, cancellationToken);

            return groups.Select(group => new GroupBaseResponse
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
            }).ToList();
        }
    }
}
