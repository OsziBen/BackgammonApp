using MediatR;

namespace Application.Tournaments.Commands.AddTournamentParticipant
{
    public record AddTournamentParticipantCommand(Guid TournamentId, string UserName, Guid UserId) : IRequest<Unit>;
}
