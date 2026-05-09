using Application.Interfaces.Repository.Tournament;
using Application.Shared;
using Application.Tournament.Responses;
using MediatR;

namespace Application.Tournament.Commands.GetTournamentById
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

            return new TournamentBaseResponse
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
            };
        }
    }
}
