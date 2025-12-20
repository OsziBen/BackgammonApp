using Common.Enums.BoardState;
using Common.Enums.GameSession;

namespace Application.GameSessions.Responses
{
    public class GameSessionSnapshotResponse
    {
        public Guid SessionId { get; set; }
        public Guid PlayerId { get; set; }
        public PlayerColor PlayerColor { get; set; }

        public GamePhase CurrentPhase { get; set; }
        public Guid? CurrentPlayerId { get; set; }

        public string? BoardStateJson { get; set; }

        public bool IsRejoin { get; set; }
    }
}
