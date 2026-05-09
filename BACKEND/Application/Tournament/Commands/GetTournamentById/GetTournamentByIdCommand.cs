using Application.Tournament.Responses;
using MediatR;

namespace Application.Tournament.Commands.GetTournamentById
{
    public record GetTournamentByIdCommand(Guid TournamentId, Guid UserId) : IRequest<TournamentBaseResponse>;
}
