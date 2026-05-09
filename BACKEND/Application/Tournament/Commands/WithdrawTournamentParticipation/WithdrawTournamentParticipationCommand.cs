using MediatR;

namespace Application.Tournament.Commands.WithdrawTournamentParticipation
{
    public record WithdrawTournamentParticipationCommand(Guid TournamentId, Guid UserId) : IRequest<Unit>;
}
