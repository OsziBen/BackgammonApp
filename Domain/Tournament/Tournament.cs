using Common.Enums.Tournament;
using Common.Models;

namespace Domain.Tournament
{
    public class Tournament : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

        public TournamentType Type { get; set; }
        public TournamentStatus Status { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public Guid OrganizerUserId { get; set; }
        public Guid RulesTemplateId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }


        public User.User OrganizerUser { get; set; } = null!;
        public RulesTemplate.RulesTemplate RulesTemplate { get; set; } = null!;
        public ICollection<TournamentParticipant.TournamentParticipant> Participants { get; set; } = [];
        public ICollection<TournamentRound.TournamentRound> Rounds { get; set; } = [];
        public ICollection<TournamentStanding.TournamentStanding> Standings { get; set; } = [];
    }
}
