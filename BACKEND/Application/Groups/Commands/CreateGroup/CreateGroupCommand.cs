using Application.Groups.Responses;
using Common.Enums.Group;
using MediatR;

namespace Application.Groups.Commands.CreateGroup
{
    public record CreateGroupCommand(
        Guid UserId,
        string Name,
        string Description,
        GroupVisibility Visibility,
        GroupSizePreset SizePreset)
        : IRequest<GroupBaseResponse>;

}
