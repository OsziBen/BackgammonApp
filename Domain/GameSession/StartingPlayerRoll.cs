using Common.Enums;
using Common.Exceptions;

namespace Domain.GameSession
{
    public sealed class StartingPlayerRoll
    {
        public int Player1Roll { get; }
        public int Player2Roll { get; }

        public StartingPlayerRoll(int roll1, int roll2)
        {
            if (roll1 == roll2)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidDiceRollValues,
                    "Starting player rolls must be distinct.");
            }

            Player1Roll = roll1;
            Player2Roll = roll2;
        }
    }
}
