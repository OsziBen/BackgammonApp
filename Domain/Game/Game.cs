using Common.Enums.Game;
using Common.Models;

namespace Domain.Game
{
    public class Game : BaseEntity
    {
        public Guid MatchId { get; set; }
        public int GameNumber { get; set; }

        public Guid? StartingPlayerId { get; set; }
        public Guid? WinnerId { get; set; }

        public int WhitePlayerRemainingSeconds { get; set; }
        public int BlackPlayerRemainingSeconds { get; set; }

        public GameResultType? WhitePlayerScore { get; set; }
        public GameResultType? BlackPlayerScore { get; set; }

        public Guid? DoublingCubeOwnerId { get; set; }
        public int DoublingCubeValue { get; set; }

        public bool IsCrawfordActive { get; set; }
        public bool IsFinished { get; set; }
        public bool IsTimeOutLoss { get; set; }
        public bool IsDeleted { get; set; }
        public string? Notes { get; set; }

        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset? FinishedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public Match.Match Match { get; set; } = null!;
        public User.User? Winner { get; set; }
        public User.User? StartingPlayer { get; set; }
        public User.User? DoublingCubeOwner { get; set; }
        public ICollection<PlayerTurn.PlayerTurn> PlayerMoves { get; set; } = [];
        public ICollection<BoardState.BoardState> BoardStates { get; set; } = [];
    }
}
