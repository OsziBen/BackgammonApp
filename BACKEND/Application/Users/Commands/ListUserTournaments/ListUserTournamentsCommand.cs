using Application.Tournament.Responses;
using MediatR;

namespace Application.Users.Commands.ListUserTournaments
{
    public record ListUserTournamentsCommand(Guid UserId) : IRequest<List<TournamentBaseResponse>>;
}
