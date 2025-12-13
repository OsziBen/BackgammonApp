using Common.Models;

namespace Domain.DiceRoll
{
    public class DiceRoll : BaseEntity
    {
        public Guid PlayerTurnId { get; set; }

        public int Die1 { get; set; }
        public int Die2 { get; set; }
        public bool IsDouble { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public PlayerTurn.PlayerTurn PlayerTurn { get; set; } = null!;
    }
}
