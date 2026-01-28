namespace Domain.GameLogic.Generators
{
    public interface IMoveSequenceGenerator
    {
        IReadOnlyList<MoveSequence> Generate(
            BoardState state,
            DiceRoll roll);
    }
}
