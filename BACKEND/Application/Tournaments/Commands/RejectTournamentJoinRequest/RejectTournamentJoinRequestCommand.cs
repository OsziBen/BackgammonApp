using MediatR;

namespace Application.Tournaments.Commands.RejectTournamentJoinRequest
{
    public record RejectTournamentJoinRequestCommand(Guid TournamentId, Guid UserId, Guid RequestId) : IRequest<Unit>;
}
