namespace Domain.GameLogic
{
    public record MoveSequence(IReadOnlyList<Move> Moves)
    {
        public virtual bool Equals(MoveSequence? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (Moves.Count != other.Moves.Count)
            {
                return false;
            }

            for (int i = 0; i < Moves.Count; i++)
            {
                if (!Moves[i].Equals(other.Moves[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Moves.Aggregate(
                    17,
                    (hash, move) => hash * 23 + move.GetHashCode());
            }
        }
    }
}
