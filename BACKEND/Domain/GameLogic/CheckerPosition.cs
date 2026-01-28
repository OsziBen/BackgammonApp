using Common.Enums.BoardState;

namespace Domain.GameLogic
{
    public class CheckerPosition
    {
        public PlayerColor? Owner { get; }
        public int Count { get; }

        public CheckerPosition(
            PlayerColor? owner,
            int count)
        {
            Owner = owner;
            Count = count;
        }

        public CheckerPosition Clone()
            => new CheckerPosition(Owner, Count);

        public CheckerPosition With(
            PlayerColor? owner,
            int count)
            => new CheckerPosition(Owner, count);
    }
}
