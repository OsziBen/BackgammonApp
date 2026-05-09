using MediatR;

namespace Application.Tournament.Commands.ApproveTournamentJoinRequest
{
    public record ApproveTournamentJoinRequestCommand(Guid TournamentId, Guid UserId, Guid RequestId) : IRequest<Unit>;
}
