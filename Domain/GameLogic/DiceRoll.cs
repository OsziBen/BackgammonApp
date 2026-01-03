using Common.Enums;
using Common.Exceptions;

namespace Domain.GameLogic
{
    public class DiceRoll
    {
        public IReadOnlyList<int> Values { get; }

        public bool IsDouble { get; }

        public DiceRoll(IEnumerable<int> values)
        {
            var dice = values.ToList();

            if (dice.Count != 2)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidDiceRoll,
                    "Dice roll must contain exactly 2 values");
            }

            if (dice.Any(d => d < 1 || d > 6))
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidDiceRollValues,
                    "Dice values must be between 1 and 6");
            }

            IsDouble = dice[0] == dice[1];

            Values = IsDouble
                ? new[] { dice[0], dice[0], dice[0], dice[0] }
                : dice;
        }

        public int MaxMoves => Values.Count;

        public IEnumerable<IEnumerable<int>> GetPermutations()
        {
            if (IsDouble)
            {
                yield return Values;
                yield break;
            }

            yield return Values;
            yield return new[] { Values[1], Values[0] };
        }
    }
}
