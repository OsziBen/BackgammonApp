using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GameSession;

namespace Application.GameSessions.Guards
{
    public static class GameSessionStateGuards
    {
        public static void EnsureDiceStateValid(GameSession session)
        {
            if (session.CurrentPhase == GamePhase.MoveCheckers &&
                (session.LastDiceRoll == null || session.RemainingMoves == null))
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidGameState,
                    "Dice state is missing while in MoveCheckers phase");
            }
        }

        public static void EnsureNoActiveDice(GameSession session)
        {
            if (session.LastDiceRoll != null || session.RemainingMoves != null)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidGameState,
                    "Dice already rolled for this turn");
            }
        }
    }
}
