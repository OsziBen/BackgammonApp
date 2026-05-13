using Application.Groups.Helpers;
using Application.Interfaces.Repository.GroupJoinRequest;
using Application.Interfaces.Repository.Tournament;
using Application.Interfaces.Repository.TournamentJoinRequest;
using Application.Interfaces.Repository.TournamentParticipant;
using Application.Tournaments.Helpers;
using Application.Tournaments.Responses;
using MediatR;

namespace Application.Tournaments.Commands.GetAllTournaments
{
    public class GetAllTournamentsCommandHandler : IRequestHandler<GetAllTournamentsCommand, List<TournamentBaseResponse>>
    {
        private readonly ITournamentReadRepository _tournamentReadRepository;
        private readonly ITournamentJoinRequestReadRepository _tournamentJoinRequestReadRepository;
        private readonly ITournamentParticipantReadRepository _tournamentParticipantReadRepository;

        public GetAllTournamentsCommandHandler(
            ITournamentReadRepository tournamentReadRepository,
            ITournamentJoinRequestReadRepository tournamentJoinRequestReadRepository,
            ITournamentParticipantReadRepository tournamentParticipantReadRepository)
        {
            _tournamentReadRepository = tournamentReadRepository;
            _tournamentJoinRequestReadRepository = tournamentJoinRequestReadRepository;
            _tournamentParticipantReadRepository = tournamentParticipantReadRepository;
        }

        public async Task<List<TournamentBaseResponse>> Handle(GetAllTournamentsCommand request, CancellationToken cancellationToken)
        {
            var tournaments = await _tournamentReadRepository
                .GetAllPublicAsync(cancellationToken);

            var participations = await _tournamentParticipantReadRepository
                .GetTournamentParticipationsByUserIdAsync(request.UserId, cancellationToken);

            var participationLookup = participations.ToDictionary(
                p => p.TournamentId,
                p => p);

            var pendingJoinRequests = await _tournamentJoinRequestReadRepository
                .GetAllPendingByUserIdAsync(
                    request.UserId,
                    cancellationToken);

            var pendingGroupIds = pendingJoinRequests
                .Select(x => x.TournamentId)
                .ToHashSet();

            return tournaments.Select(tournament =>
            {
                participationLookup.TryGetValue(tournament.Id, out var membership);

                var hasPendingRequest = pendingGroupIds.Contains(tournament.Id);

                return TournamentResponseMapper.ToBaseResponse(
                    tournament,
                    request.UserId,
                    hasPendingRequest);
            }).ToList();
        }
    }
}
