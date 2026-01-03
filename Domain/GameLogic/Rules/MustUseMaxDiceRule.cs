namespace Domain.GameLogic.Rules
{
    public class MustUseMaxDiceRule : IMoveSequenceRule
    {
        public IEnumerable<MoveSequence> Apply(
            IEnumerable<MoveSequence> sequences,
            DiceRoll roll)
        {
            var list = sequences.ToList();

            if (list.Count == 0)
            {
                return list;
            }

            var maxUsedDice = list.Max(s => s.Moves.Count);

            return list
                .Where(s => s.Moves.Count == maxUsedDice);
        }
    }
}
