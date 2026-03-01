using Application.Interfaces.Common;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.AppRole;
using Application.Interfaces.Repository.User;
using Application.Shared.Time;
using Application.Users.Responses;
using Common.Constants;
using Common.Enums;
using Common.Exceptions;
using Domain.User;
using MediatR;

namespace Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, TokenResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAppRoleReadRepository _appRoleReadRepository;

        public RegisterUserCommandHandler(
            IUnitOfWork uow,
            IUserReadRepository userReadRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IDateTimeProvider dateTimeProvider,
            IAppRoleReadRepository appRoleReadRepository)
        {
            _uow = uow;
            _userReadRepository = userReadRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _dateTimeProvider = dateTimeProvider;
            _appRoleReadRepository = appRoleReadRepository;
        }

        public async Task<TokenResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userReadRepository.ExistsByEmailAddressAsync(request.EmailAddress, cancellationToken))
            {
                throw new BusinessRuleException(FunctionCode.UserWithEmailAlreadyExists,
                    $"User with Email {request.EmailAddress} already exists.");
            }

            if (await _userReadRepository.ExistsByUserNameAsync(request.UserName, cancellationToken))
            {
                throw new BusinessRuleException(FunctionCode.UserWithUserNameAlreadyExists,
                    $"User with UserName {request.UserName} already exists.");
            }

            var now = _dateTimeProvider.UtcNow;
            var passwordHash = _passwordHasher.Hash(request.Password);

            var role = await _appRoleReadRepository.GetByNameAsync(AppRoleConstants.User, cancellationToken);

            if (role == null)
            {
                throw new NotFoundException(FunctionCode.ResourceNotFound,
                    $"Role with Name {AppRoleConstants.User} not found.");
            }

            var user = UserFactory.CreateAppUser(
                request.FirstName,
                request.LastName,
                request.UserName,
                request.EmailAddress,
                passwordHash,
                request.DateOfBirth,
                role.Id,
                now);

            await _uow.UsersWrite.AddAsync(user, cancellationToken);
            await _uow.CommitAsync(cancellationToken);

            return new TokenResponse { 
                JwtToken = _tokenService.GenerateAccessToken(user, role.Name)
            };
        }
    }
}
