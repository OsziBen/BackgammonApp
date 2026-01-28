using Common.Models;

namespace Domain.RulesTemplate
{
    public class RulesTemplate : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Guid? AuthorId { get; set; }

        public int TargetScore { get; set; }
        public bool UseClock { get; set; }
        public int? MatchTimePerPlayerInSeconds { get; set; }
        public int? StartOfTurnDelayPerPlayerInSeconds { get; set; }
        public bool CrawfordRuleEnabled { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDeleted { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public User.User? Author { get; set; }
        public ICollection<Match.Match> Matches { get; set; } = [];
    }
}
