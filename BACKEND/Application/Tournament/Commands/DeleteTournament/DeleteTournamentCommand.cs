using MediatR;

namespace Application.Tournament.Commands.DeleteTournament
{
    public record DeleteTournamentCommand() : IRequest<Unit>;
}
