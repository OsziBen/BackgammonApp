using Application.Tournament.Responses;
using MediatR;

namespace Application.Tournament.Commands.ListTournamentJoinRequests
{
    public record ListTournamentJoinRequestsCommand(Guid TournamentId) : IRequest<List<TournamentJoinRequestResponse>>;
}
