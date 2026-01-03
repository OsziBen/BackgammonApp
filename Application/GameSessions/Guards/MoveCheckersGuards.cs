using Application.GameSessions.Requests;
using Common.Enums;
using Common.Exceptions;
using Domain.GameSession;

namespace Application.GameSessions.Guards
{
    public static class MoveCheckersGuards
    {
        public static void EnsureDiceRolled(GameSession session)
        {
            if (session.LastDiceRoll == null)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidGameState,
                    "Dice not rolled");
            }
        }

        public static void EnsureMovesProvided(IReadOnlyList<MoveDto> moves)
        {
            if (moves == null || moves.Count == 0)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidMove,
                    "Moves list cannot be null");
            }
        }
    }
}
