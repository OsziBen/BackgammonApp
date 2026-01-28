using Common.Models;

namespace Domain.CheckerMove
{
    public class CheckerMove : BaseEntity
    {
        public Guid PlayerTurnId { get; set; }
        public int OrderWithinTurn { get; set; }  // hányadik lépés adott körben

        public int FromPoint { get; set; }
        public int ToPoint { get; set; }
        public int PipsUsed { get; set; }    // érték, nem a kocka száma!

        public bool IsHit { get; set; }
        public bool IsBearOff { get; set; }

        public Guid? ActualBoardStateId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public PlayerTurn.PlayerTurn PlayerTurn { get; set; } = null!;
    }
}
