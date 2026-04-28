using MediatR;

namespace Application.Groups.Commands.DeleteGroup
{
    public record DeleteGroupCommand(Guid GroupId) : IRequest<Unit>;
}
