using MediatR;

namespace Application.Tournament.Commands.JoinTournament
{
    public record JoinTournamentCommand(Guid TournamentId, Guid UserId) : IRequest<Unit>;
}
