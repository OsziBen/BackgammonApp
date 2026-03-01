using MediatR;

namespace Application.GameSessions.Commands.RollDice
{
    public record RollDiceCommand(Guid SessionId, Guid PlayerId) : IRequest<Unit>;
}
