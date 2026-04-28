using Application.Groups.Responses;
using Common.Enums.Group;
using MediatR;

namespace Application.Groups.Commands.EditGroup
{
    public record EditGroupCommand(
        Guid GroupId,
        string Name,
        string Description,
        GroupVisibility Visibility,
        GroupSizePreset SizePreset) : IRequest<BaseGroupResponse>;
}
