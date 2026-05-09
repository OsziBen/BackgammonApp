using MediatR;

namespace Application.Tournament.Commands.AddTournamentParticipant
{
    public record AddTournamentParticipantCommand(Guid TournamentId, string UserName, Guid UserId) : IRequest<Unit>;
}
