using MediatR;

namespace Application.Tournaments.Commands.DeleteTournament
{
    public record DeleteTournamentCommand(Guid TournamentId, Guid UserId) : IRequest<Unit>;
}
