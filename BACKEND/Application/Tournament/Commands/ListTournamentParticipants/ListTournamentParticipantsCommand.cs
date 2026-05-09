using Application.Tournament.Responses;
using MediatR;

namespace Application.Tournament.Commands.ListTournamentParticipants
{
    public record ListTournamentParticipantsCommand(Guid TournamentId) : IRequest<TournamentParticipantsResponse>;
}
