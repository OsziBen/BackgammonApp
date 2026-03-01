using Application.Users.Responses;
using MediatR;

namespace Application.Users.Commands.RegisterUser
{
    public record RegisterUserCommand(
        string FirstName,
        string LastName,
        string UserName,
        string EmailAddress,
        string Password,
        DateOnly DateOfBirth)
        : IRequest<TokenResponse>;
}
