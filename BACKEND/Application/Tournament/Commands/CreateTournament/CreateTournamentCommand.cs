using MediatR;

namespace Application.Tournament.Commands.CreateTournament
{
    public record CreateTournamentCommand() : IRequest<Unit>;
}
