using MediatR;

namespace Application.Tournament.Commands.RemoveTournamentParticipant
{
    public record RemoveTournamentParticipantCommand(Guid TournamentId, Guid UserId) : IRequest<Unit>;
}
