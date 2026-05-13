using Application.Interfaces.Repository.Tournament;
using Application.Interfaces.Repository.TournamentJoinRequest;
using Application.Interfaces.Repository.TournamentParticipant;
using Application.Shared;
using Application.Tournaments.Helpers;
using Application.Tournaments.Responses;
using Domain.Tournament;
using MediatR;

namespace Application.Tournaments.Commands.GetTournamentById
{
    public class GetTournamentByIdCommandHandler : IRequestHandler<GetTournamentByIdCommand, TournamentBaseResponse>
    {
        private readonly ITournamentReadRepository _tournamentReadRepository;
        private readonly ITournamentJoinRequestReadRepository _tournamentJoinRequestReadRepository;
        private readonly ITournamentParticipantReadRepository _tournamentParticipantReadRepository;

        public GetTournamentByIdCommandHandler(
            ITournamentReadRepository tournamentReadRepository,
            ITournamentJoinRequestReadRepository tournamentJoinRequestReadRepository,
            ITournamentParticipantReadRepository tournamentParticipantReadRepository)
        {
            _tournamentReadRepository = tournamentReadRepository;
            _tournamentJoinRequestReadRepository = tournamentJoinRequestReadRepository;
            _tournamentParticipantReadRepository = tournamentParticipantReadRepository;
        }

        public async Task<TournamentBaseResponse> Handle(GetTournamentByIdCommand request, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentReadRepository
                .GetByIdAsync(request.TournamentId, cancellationToken)
                .GetOrThrowAsync(nameof(Tournament), request.TournamentId);

            var participations = await _tournamentParticipantReadRepository
                .GetTournamentParticipationsByUserIdAsync(request.UserId, cancellationToken);

            var participationLookup = participations.ToDictionary(
                p => p.TournamentId,
                p => p);

            var pendingJoinRequests = await _tournamentJoinRequestReadRepository
                .GetAllPendingByUserIdAsync(
                    request.UserId,
                    cancellationToken);

            var hasPendingRequest = pendingJoinRequests
                .Any(jr => jr.TournamentId == request.TournamentId);

            return TournamentResponseMapper.ToBaseResponse(
                tournament,
                request.UserId,
                hasPendingRequest
            );
        }
    }
}
