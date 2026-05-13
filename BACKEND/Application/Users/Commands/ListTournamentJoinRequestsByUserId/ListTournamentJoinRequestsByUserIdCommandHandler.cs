using Application.Interfaces.Repository.TournamentJoinRequest;
using Application.Users.Responses;
using MediatR;

namespace Application.Users.Commands.ListTournamentJoinRequestsByUserId
{
    public class ListTournamentJoinRequestsByUserIdCommandHandler : IRequestHandler<ListTournamentJoinRequestsByUserIdCommand, List<UserTournamentJoinRequestResponse>>
    {
        private readonly ITournamentJoinRequestReadRepository _tournamentJoinRequestReadRepository;

        public ListTournamentJoinRequestsByUserIdCommandHandler(ITournamentJoinRequestReadRepository tournamentJoinRequestReadRepository)
        {
            _tournamentJoinRequestReadRepository = tournamentJoinRequestReadRepository;
        }

        public async Task<List<UserTournamentJoinRequestResponse>> Handle(ListTournamentJoinRequestsByUserIdCommand request, CancellationToken cancellationToken)
        {
            var joinRequests = await _tournamentJoinRequestReadRepository
                .GetAllByUserIdAsync(request.UserId, cancellationToken);

            return joinRequests.Select(joinRequest => new UserTournamentJoinRequestResponse
            {
                Id = joinRequest.Id,
                TournamentName = joinRequest.Tournament.Name,
                StartDate = joinRequest.Tournament.StartDate,
                EndDate = joinRequest.Tournament.EndDate,
                Deadline = joinRequest.Tournament.Deadline,
                OrganizerUserName = joinRequest.Tournament.OrganizerUser.UserName,
                Status = joinRequest.Status.ToString(),
                CreatedAt = joinRequest.CreatedAt
            }).ToList();
        }
    }
}
