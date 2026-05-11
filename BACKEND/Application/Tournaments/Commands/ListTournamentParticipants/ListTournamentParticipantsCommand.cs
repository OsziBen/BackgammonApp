using Application.Tournaments.Responses;
using MediatR;

namespace Application.Tournaments.Commands.ListTournamentParticipants
{
    public record ListTournamentParticipantsCommand(Guid TournamentId) : IRequest<TournamentParticipantsResponse>;
}
