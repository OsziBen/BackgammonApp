using Application.Groups.Responses;
using MediatR;

namespace Application.Groups.Commands.GetGroupById
{
    public record GetGroupByIdCommand(Guid GroupId) : IRequest<BaseGroupResponse>;
}
