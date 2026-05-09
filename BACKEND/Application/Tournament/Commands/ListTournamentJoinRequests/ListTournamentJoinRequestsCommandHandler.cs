using Application.Interfaces.Repository.TournamentJoinRequest;
using Application.Tournament.Responses;
using MediatR;

namespace Application.Tournament.Commands.ListTournamentJoinRequests
{
    public class ListTournamentJoinRequestsCommandHandler : IRequestHandler<ListTournamentJoinRequestsCommand, List<TournamentJoinRequestResponse>>
    {
        private readonly ITournamentJoinRequestReadRepository _tournamentJoinRequestReadRepository;

        public ListTournamentJoinRequestsCommandHandler(ITournamentJoinRequestReadRepository tournamentJoinRequestReadRepository)
        {
            _tournamentJoinRequestReadRepository = tournamentJoinRequestReadRepository;
        }

        public async Task<List<TournamentJoinRequestResponse>> Handle(ListTournamentJoinRequestsCommand request, CancellationToken cancellationToken)
        {
            var tournamentJoinRequests = await _tournamentJoinRequestReadRepository
                .GetAllByTournamentIdAsync(request.TournamentId, cancellationToken);

            return tournamentJoinRequests.Select(joinRequest => new TournamentJoinRequestResponse
            {
                Id = joinRequest.Id,
                UserName = joinRequest.User.UserName,
                Status = joinRequest.Status.ToString(),
                CreatedAt = joinRequest.CreatedAt,
                ReviewedAt = joinRequest.ReviewedAt,
                ReviewedByUser = joinRequest.ReviewedByUser?.UserName
            }).ToList();
        }
    }
}
