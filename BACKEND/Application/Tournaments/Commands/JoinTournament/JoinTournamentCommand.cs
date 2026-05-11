using MediatR;

namespace Application.Tournaments.Commands.JoinTournament
{
    public record JoinTournamentCommand(Guid TournamentId, Guid UserId) : IRequest<Unit>;
}
