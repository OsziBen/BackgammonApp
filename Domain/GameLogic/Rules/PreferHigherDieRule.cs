
namespace Domain.GameLogic.Rules
{
    public class PreferHigherDieRule : IMoveSequenceRule
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

            var highestDie = list
                .SelectMany(sequence => sequence.Moves)
                .Max(move => move.Die);

            return list
                .Where(s =>
                    s.Moves.Any(move => move.Die == highestDie));
        }
    }
}
