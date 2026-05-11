using MediatR;

namespace Application.Tournaments.Commands.WithdrawTournamentParticipation
{
    public record WithdrawTournamentParticipationCommand(Guid TournamentId, Guid UserId) : IRequest<Unit>;
}
