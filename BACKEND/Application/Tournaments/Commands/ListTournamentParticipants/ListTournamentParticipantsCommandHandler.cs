using Application.Interfaces.Repository.Tournament;
using Application.Interfaces.Repository.TournamentParticipant;
using Application.Shared;
using Application.Tournaments.Responses;
using Domain.Tournament;
using MediatR;

namespace Application.Tournaments.Commands.ListTournamentParticipants
{
    public class ListTournamentParticipantsCommandHandler : IRequestHandler<ListTournamentParticipantsCommand, TournamentParticipantsResponse>
    {
        private readonly ITournamentReadRepository _tournamentReadRepository;
        private readonly ITournamentParticipantReadRepository _tournamentParticipantReadRepository;

        public ListTournamentParticipantsCommandHandler(
            ITournamentReadRepository tournamentReadRepository,
            ITournamentParticipantReadRepository tournamentParticipantReadRepository)
        {
            _tournamentReadRepository = tournamentReadRepository;
            _tournamentParticipantReadRepository = tournamentParticipantReadRepository;
        }

        public async Task<TournamentParticipantsResponse> Handle(ListTournamentParticipantsCommand request, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentReadRepository
                .GetByIdAsync(request.TournamentId, cancellationToken)
                .GetOrThrowAsync(nameof(Tournament), request.TournamentId);

            var participants = await _tournamentParticipantReadRepository
                .GetUsersByTournamentIdAsync(request.TournamentId, cancellationToken);

            return new TournamentParticipantsResponse
            {
                Participants = participants.Select(p => new TournamentParticipantBaseResponse
                {
                    TournamentId = p.TournamentId,
                    UserId = p.UserId,
                    Status = p.Status.ToString(),
                    DisplayName = p.DisplayName,
                    Email = p.Email,
                    Notes = p.Notes,
                    CreatedAt = p.CreatedAt,
                }).ToList(),
                MaxParticipantsNumber = tournament.MaxParticipants,
                CurrentParticipantsNumber = participants.Count,
            };
        }
    }
}
