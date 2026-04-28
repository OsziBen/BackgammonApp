using Application.Groups.Responses;
using Application.Interfaces.Repository.Group;
using Application.Shared;
using Domain.Group;
using MediatR;

namespace Application.Groups.Commands.GetGroupById
{
    public class GetGroupByIdCommandHandler : IRequestHandler<GetGroupByIdCommand, BaseGroupResponse>
    {
        private readonly IGroupReadRepository _groupReadRepository;

        public GetGroupByIdCommandHandler(IGroupReadRepository groupReadRepository)
        {
            _groupReadRepository = groupReadRepository;
        }

        public async Task<BaseGroupResponse> Handle(GetGroupByIdCommand request, CancellationToken cancellationToken)
        {
            var group = await _groupReadRepository
                .GetByIdAsync(request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(Group), request.GroupId);

            return new BaseGroupResponse
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
                CreatedAt = group.CreatedAt,
            };
        }
    }
}
