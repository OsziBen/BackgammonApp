using Application.Tournaments.Responses;
using MediatR;

namespace Application.Tournaments.Commands.GetAllTournaments
{
    public record GetAllTournamentsCommand(Guid UserId) : IRequest<List<TournamentBaseResponse>>;
}
