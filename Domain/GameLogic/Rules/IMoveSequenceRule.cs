namespace Domain.GameLogic.Rules
{
    public interface IMoveSequenceRule
    {
        IEnumerable<MoveSequence> Apply(
            IEnumerable<MoveSequence> sequences,
            DiceRoll roll);
    }
}
