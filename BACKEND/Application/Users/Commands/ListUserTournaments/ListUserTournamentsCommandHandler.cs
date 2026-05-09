using Application.Interfaces.Repository.Tournament;
using Application.Tournament.Responses;
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
            var tournaments = await _tournamentReadRepository.GetAllByUserIdAsync(request.UserId, cancellationToken);

            return tournaments.Select(tournament => new TournamentBaseResponse
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                Type = tournament.Type.ToString(),
                Visibility = tournament.Visibility.ToString(),
                Status = tournament.Status.ToString(),
                MaxParticipants = tournament.MaxParticipants,
                StartDate = tournament.StartDate,
                EndDate = tournament.EndDate,
                Deadline = tournament.Deadline,
                OrganizerUserName = tournament.OrganizerUser.UserName
            }).ToList();
        }
    }
}
