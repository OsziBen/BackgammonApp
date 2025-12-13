using Common.Enums.BoardState;
using Common.Models;
using Domain.PlayerTurn;

namespace Domain.BoardState
{
    public class BoardState : BaseEntity
    {
        public Guid GameId { get; set; }
        public Guid? PlayerTurnId { get; set; }
        public Guid? CheckerMoveId { get; set; }

        public required string PositionsJson { get; set; }
        public int Order { get; set; }

        public int BarWhite { get; set; }
        public int BarBlack { get; set; }
        public int OffWhite { get; set; }
        public int OffBlack { get; set; }

        public PlayerColor CurrentPlayer { get; set; }
        public int CubeValue { get; set; }
        public Guid? CubeOwnerId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
