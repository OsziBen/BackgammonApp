using Common.Enums;
using Common.Exceptions;
using Domain.GameSession;

namespace Application.GameSessions.Guards
{
    public static class JoinGameSessionGuards
    {
        public static void EnsureSessionNotFull(GameSession session)
        {
            if (session.Players.Count >= 2)
            {
                throw new BusinessRuleException(
                    FunctionCode.SessionFull,
                    "Session is full");
            }
        }
    }
}
