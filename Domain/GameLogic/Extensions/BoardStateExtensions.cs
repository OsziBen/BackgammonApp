namespace Domain.GameLogic.Extensions
{
    public static class BoardStateExtensions
    {
        public static BoardState Apply(
            this BoardState state,
            Move move)
        {
            return BoardStateApplier.Apply(state, move);
        }
    }
}
