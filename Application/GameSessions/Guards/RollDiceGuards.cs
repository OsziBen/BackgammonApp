using Common.Enums;
using Common.Exceptions;
using Domain.GameSession;

namespace Application.GameSessions.Guards
{
    public static class RollDiceGuards
    {
        public static void EnsureCurrentPlayer(
            GameSession session,
            Guid playerId)
        {
            if (session.CurrentPlayerId != playerId)
            {
                throw new BusinessRuleException(
                    FunctionCode.NotYourTurn,
                    "Only the current player can roll dice");
            }
        }
    }
}
