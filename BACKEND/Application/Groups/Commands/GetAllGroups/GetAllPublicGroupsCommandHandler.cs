using Application.Groups.Responses;
using Application.Interfaces.Repository.Group;
using MediatR;

namespace Application.Groups.Commands.GetAllGroups
{
    public class GetAllPublicGroupsCommandHandler : IRequestHandler<GetAllPublicGroupsCommand, List<BaseGroupResponse>>
    {
        private readonly IGroupReadRepository _groupReadRepository;

        public GetAllPublicGroupsCommandHandler(IGroupReadRepository groupReadRepository)
        {
            _groupReadRepository = groupReadRepository;
        }

        public async Task<List<BaseGroupResponse>> Handle(GetAllPublicGroupsCommand request, CancellationToken cancellationToken)
        {
            var groups = await _groupReadRepository.GetAllPublicAsync(cancellationToken);

            return groups.Select(g => new BaseGroupResponse
            {
                Id = g.Id,
                CreatorName = g.Creator.UserName,
                Name = g.Name,
                Description = g.Description,
                Visibility = g.Visibility.ToString(),
                JoinPolicy = g.JoinPolicy.ToString(),
                SizePreset = g.SizePreset.ToString(),
                MaxMembers = g.MaxMembers,
                MaxModerators = g.MaxModerators,
                CreatedAt = g.CreatedAt,
            }).ToList();
        }
    }
}
