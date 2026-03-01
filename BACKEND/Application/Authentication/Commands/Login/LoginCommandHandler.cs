using Application.Interfaces.Common;
using Application.Interfaces.Repository.User;
using Application.Users.Responses;
using Common.Enums;
using Common.Exceptions;
using MediatR;

namespace Application.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandler(
            IUserReadRepository userReadRepository,
            ITokenService tokenService,
            IPasswordHasher passwordHasher)
        {
            _userReadRepository = userReadRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var email = request.EmailAddress.Trim().ToLower();

            var user = await _userReadRepository.GetByEmailAsync(email, cancellationToken);

            if (user == null
                || user.IsDeleted
                || user.IsBanned
                || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedException(
                    FunctionCode.AccessDenied,
                    "Invalid email or password.");
            }

            return new TokenResponse
            {
                JwtToken = _tokenService.GenerateAccessToken(user, user.AppRole.Name)
            };
        }
    }
}
