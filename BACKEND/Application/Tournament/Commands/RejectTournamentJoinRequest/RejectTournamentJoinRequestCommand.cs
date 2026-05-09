using MediatR;

namespace Application.Tournament.Commands.RejectTournamentJoinRequest
{
    public record RejectTournamentJoinRequestCommand(Guid TournamentId, Guid UserId, Guid RequestId) : IRequest<Unit>;
}
