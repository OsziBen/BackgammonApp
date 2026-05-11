using MediatR;

namespace Application.Tournaments.Commands.ApproveTournamentJoinRequest
{
    public record ApproveTournamentJoinRequestCommand(Guid TournamentId, Guid UserId, Guid RequestId) : IRequest<Unit>;
}
