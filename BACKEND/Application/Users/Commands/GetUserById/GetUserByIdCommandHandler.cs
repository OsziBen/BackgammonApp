using Application.Interfaces.Repository.User;
using Application.Shared;
using Application.Users.Responses;
using Domain.User;
using MediatR;

namespace Application.Users.Commands.GetUserById
{
    public class GetUserByIdCommandHandler : IRequestHandler<GetUserByIdCommand, UserProfileResponse>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetUserByIdCommandHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<UserProfileResponse> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository
                .GetByIdAsync(request.UserId, cancellationToken)
                .GetOrThrowAsync(nameof(User), request.UserId);

            return new UserProfileResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                EmailAddress = user.EmailAddress,
                DateOfBirth = user.DateOfBirth,
                Rating = user.Rating,
                ExperiencePoints = user.ExperiencePoints,
                CreatedAt = user.CreatedAt,
            };
        }
    }
}
