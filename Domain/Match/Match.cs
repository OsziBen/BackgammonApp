using Common.Enums.Match;
using Common.Models;

namespace Domain.Match
{
    public class Match : BaseEntity
    {
        public Guid CreatedByUserId { get; set; }
        public Common.Enums.Match.MatchType Type { get; set; }
        public MatchStatusType StatusType { get; set; }

        public Guid WhitePlayerId { get; set; }
        public Guid BlackPlayerId { get; set; }
        public Guid? WinnerId { get; set; }

        public Guid? RulesTemplateId { get; set; }
        public int TargetScore { get; set; }
        public bool UseClock { get; set; }
        public int? MatchTimePerPlayerInSeconds { get; set; }
        public int? StartOfTurnDelayPerPlayerInSeconds { get; set; }
        public bool CrawfordRuleEnabled { get; set; }

        public int CurrentGameNumber { get; set; }
        public bool IsResigned { get; set; }
        public bool IsDeleted { get; set; }
        public string? MatchCode { get; set; }
        public string? Notes { get; set; }
        
        public Guid? TournamentMatchId { get; set; }

        public DateTimeOffset ScheduledAt { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? FinishedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public User.User CreatedByUser { get; set; } = null!;
        public User.User WhitePlayer { get; set; } = null!;
        public User.User BlackPlayer { get; set; } = null!;
        public User.User? Winner { get; set; }
        public RulesTemplate.RulesTemplate? RulesTemplate { get; set; }
        public ICollection<Game.Game> Games { get; set; } = [];
    }
}
