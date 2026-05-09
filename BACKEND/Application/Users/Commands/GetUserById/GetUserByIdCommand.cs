using Application.Users.Responses;
using MediatR;

namespace Application.Users.Commands.GetUserById
{
    public record GetUserByIdCommand(Guid UserId) : IRequest<UserProfileResponse>;
}
