using Application.Tournaments.Responses;
using MediatR;

namespace Application.Tournaments.Commands.GetTournamentById
{
    public record GetTournamentByIdCommand(Guid TournamentId, Guid UserId) : IRequest<TournamentBaseResponse>;
}
