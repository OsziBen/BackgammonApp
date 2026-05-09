using MediatR;

namespace Application.Tournament.Commands.DeleteTournament
{
    public record DeleteTournamentCommand(Guid TournamentId, Guid UserId) : IRequest<Unit>;
}
