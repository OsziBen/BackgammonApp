using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;

namespace Domain.GameSession
{
    public static class GameSessionGuards
    {
        public static void EnsureNotFinished(GameSession session)
        {
            if (session.IsFinished)
            {
                throw new BusinessRuleException(
                    FunctionCode.BusinessRuleViolation,
                    "Game session already finished");
            }
        }

        public static void EnsureMaxPlayerCount(
            int actual,
            int maxAllowed)
        {
            if (actual > maxAllowed)
            {
                throw new BusinessRuleException(
                    FunctionCode.BusinessRuleViolation,
                     $"Player count exceeded. Max allowed: {maxAllowed}, actual: {actual}");
            }
        }
    }
}
