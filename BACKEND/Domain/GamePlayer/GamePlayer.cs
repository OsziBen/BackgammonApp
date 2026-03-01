using Common.Enums.BoardState;
using Common.Models;

namespace Domain.GamePlayer
{
    public class GamePlayer : BaseEntity
    {
        public Guid GameSessionId { get; set; }
        public Guid UserId { get; set; }

        public bool IsHost { get; set; }
        public PlayerColor Color { get; set; }

        public bool IsConnected { get; set; }
        public DateTimeOffset? LastConnectedAt { get; set; }

        public int? StartingRoll { get; set; }

        public GameSession.GameSession GameSession { get; set; } = null!;
        public User.User User { get; set; } = null!;


        public void Connect(DateTimeOffset now)
        {
            if (IsConnected)
            {
                return;
            }

            IsConnected = true;
            LastConnectedAt = now;
        }

        public void Disconnect(DateTimeOffset now)
        {
            if (!IsConnected)
            {
                return;
            }

            IsConnected = false;
            LastConnectedAt = now;
        }

    }
}
