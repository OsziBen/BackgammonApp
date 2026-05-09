using Application.Interfaces.Repository.Tournament;
using Application.Tournament.Responses;
using MediatR;

namespace Application.Tournament.Commands.GetAllTournaments
{
    public class GetAllTournamentsCommandHandler : IRequestHandler<GetAllTournamentsCommand, List<TournamentBaseResponse>>
    {
        private readonly ITournamentReadRepository _tournamentReadRepository;
        //private readonly ITournament

        public GetAllTournamentsCommandHandler(ITournamentReadRepository tournamentReadRepository)
        {
            _tournamentReadRepository = tournamentReadRepository;
        }

        public async Task<List<TournamentBaseResponse>> Handle(GetAllTournamentsCommand request, CancellationToken cancellationToken)
        {
            var tournaments = await _tournamentReadRepository.GetAllPublicAsync(cancellationToken);

            return tournaments.Select(t => new TournamentBaseResponse
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Type = t.Type.ToString(),
                Visibility = t.Visibility.ToString(),
                Status = t.Status.ToString(),
                MaxParticipants = t.MaxParticipants,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Deadline = t.Deadline,
                OrganizerUserName = t.OrganizerUser.UserName
            }).ToList();
        }
    }
}
