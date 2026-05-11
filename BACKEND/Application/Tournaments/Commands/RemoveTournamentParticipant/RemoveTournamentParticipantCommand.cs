using MediatR;

namespace Application.Tournaments.Commands.RemoveTournamentParticipant
{
    public record RemoveTournamentParticipantCommand(Guid TournamentId, Guid UserId) : IRequest<Unit>;
}
