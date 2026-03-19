using Common.Enums.BoardState;
using Common.Enums.Game;
using Common.Enums.GameSession;

namespace Application.GameSessions.Responses
{
    public class GameSessionSnapshotResponse
    {
        public int Version { get; set; }

        public Guid SessionId { get; set; }
        public required string SessionCode { get; set; }
        public Guid CreatedByUserId { get; set; }

        public List<PlayerSnapshot> Players { get; set; } = [];

        public GamePhase CurrentPhase { get; set; }
        public Guid? CurrentPlayerId { get; set; }

        public int[]? LastDiceRoll { get; set; }
        public string? BoardStateJson { get; set; }

        public int? DoublingCubeValue { get; set; }
        public Guid? DoublingCubeOwnerPlayerId { get; set; }
        public bool CrawfordRuleApplies { get; set; }

        public bool IsFinished { get; set; }
        public GameFinishReason? FinishReason { get; set; }
        public Guid? WinnerPlayerId { get; set; }
        public GameResultType? ResultType { get; set; }
        public int? AwardedPoints { get; set; }

        // Local context
        public Guid? LocalPlayerId { get; set; }
        public bool IsRejoin { get; set; }
    }

    public class PlayerSnapshot
    {
        public Guid PlayerId { get; set; }
        public Guid UserId { get; set; }
        public bool IsHost { get; set; }
        public PlayerColor Color { get; set; }
        public bool IsConnected { get; set; }
        public int? StartingRoll { get; set; }
    }
}
