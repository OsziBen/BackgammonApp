using Common.Enums.PlayerMove;
using Common.Models;

namespace Domain.PlayerTurn
{
    public class PlayerTurn : BaseEntity
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public int MoveNumber { get; set; }
        public PlayerMoveResultType ResultType { get; set; }

        public bool WasCubeAvailableToPlayer { get; set; }  // crawford miatt, illetve ha az ellenfélnél van, amúgy igaz lenne (középen, nála)
        public Guid? CubeOwnerAtStart { get; set; }
        public Guid? CubeOwnerAtEnd { get; set; }

        public int CubeValueAtStart { get; set; }
        public int CubeValueAtEnd { get; set; }

        public Guid? ResultingBoardStateId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Game.Game Game { get; set; } = null!;
        public User.User Player { get; set; } = null!;
        public ICollection<CubeAction.CubeAction> CubeActions { get; set; } = [];
        public DiceRollSnapshot.DiceRollSnapshot? DiceRollSnapshot { get; set; }    // nincs dobás, ha nem fogadja el a duplázást
        public ICollection<CheckerMove.CheckerMove> CheckerMoves { get; set; } = [];
    }
}
