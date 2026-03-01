using Application.Users.Responses;
using MediatR;

namespace Application.Authentication.Commands.Login
{
    public record LoginCommand(string EmailAddress, string Password) : IRequest<TokenResponse>;
}
