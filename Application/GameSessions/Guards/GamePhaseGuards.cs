using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GameSession;

namespace Application.GameSessions.Guards
{
    public static class GamePhaseGuards
    {
        public static void EnsurePhase(
            GameSession session,
            GamePhase expected)
        {
            if (session.CurrentPhase != expected)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidGamePhase,
                    $"Expected phase {expected}, but was {session.CurrentPhase}");
            }
        }
    }
}
