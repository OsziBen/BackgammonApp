using Application.Tournaments.Responses;
using MediatR;

namespace Application.Tournaments.Commands.ListTournamentJoinRequests
{
    public record ListTournamentJoinRequestsCommand(Guid TournamentId) : IRequest<List<TournamentJoinRequestResponse>>;
}
