using Application.Interfaces.Repository.Tournament;
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

        public GetTournamentByIdCommandHandler(ITournamentReadRepository tournamentReadRepository)
        {
            _tournamentReadRepository = tournamentReadRepository;
        }

        public async Task<TournamentBaseResponse> Handle(GetTournamentByIdCommand request, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentReadRepository
                .GetByIdAsync(request.TournamentId, cancellationToken)
                .GetOrThrowAsync(nameof(Tournament), request.TournamentId);

            return TournamentResponseMapper.ToBaseResponse(
                tournament,
                request.UserId
            );
        }
    }
}
