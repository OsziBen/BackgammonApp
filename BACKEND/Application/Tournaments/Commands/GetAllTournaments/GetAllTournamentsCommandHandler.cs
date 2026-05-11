using Application.Interfaces.Repository.Tournament;
using Application.Tournaments.Helpers;
using Application.Tournaments.Responses;
using MediatR;

namespace Application.Tournaments.Commands.GetAllTournaments
{
    public class GetAllTournamentsCommandHandler : IRequestHandler<GetAllTournamentsCommand, List<TournamentBaseResponse>>
    {
        private readonly ITournamentReadRepository _tournamentReadRepository;

        public GetAllTournamentsCommandHandler(ITournamentReadRepository tournamentReadRepository)
        {
            _tournamentReadRepository = tournamentReadRepository;
        }

        public async Task<List<TournamentBaseResponse>> Handle(GetAllTournamentsCommand request, CancellationToken cancellationToken)
        {
            var tournaments = await _tournamentReadRepository
                .GetAllPublicAsync(cancellationToken);

            return tournaments
                .Select(t => TournamentResponseMapper.ToBaseResponse(t, request.UserId))
                .ToList();
        }
    }
}
