using MediatR;

namespace Application.Tournament.Commands.UpdateTournament
{
    public record UpdateTournamentCommand : IRequest<Unit>;
}
