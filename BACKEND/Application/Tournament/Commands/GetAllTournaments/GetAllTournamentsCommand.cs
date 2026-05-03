using Application.Tournament.Responses;
using MediatR;

namespace Application.Tournament.Commands.GetAllTournaments
{
    public record GetAllTournamentsCommand() : IRequest<List<TournamentBaseResponse>>;
}
