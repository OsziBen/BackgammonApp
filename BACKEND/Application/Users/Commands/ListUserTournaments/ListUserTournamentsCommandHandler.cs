using Application.Interfaces.Repository.Tournament;
using Application.Tournaments.Helpers;
using Application.Tournaments.Responses;
using MediatR;

namespace Application.Users.Commands.ListUserTournaments
{
    public class ListUserTournamentsCommandHandler : IRequestHandler<ListUserTournamentsCommand, List<TournamentBaseResponse>>
    {
        private readonly ITournamentReadRepository _tournamentReadRepository;

        public ListUserTournamentsCommandHandler(ITournamentReadRepository tournamentReadRepository)
        {
            _tournamentReadRepository = tournamentReadRepository;
        }

        public async Task<List<TournamentBaseResponse>> Handle(ListUserTournamentsCommand request, CancellationToken cancellationToken)
        {
            var tournaments = await _tournamentReadRepository
                .GetAllByUserIdAsync(request.UserId, cancellationToken);

            return tournaments
                .Select(t => TournamentResponseMapper.ToBaseResponse(t, request.UserId))
                .ToList();
        }
    }
}
