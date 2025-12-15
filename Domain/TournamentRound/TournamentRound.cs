using Common.Models;

namespace Domain.TournamentRound
{
    public class TournamentRound : BaseEntity
    {
        public Guid TournamentId { get; set; }

        public int RoundNumber { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public bool IsFinished { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }


        public Tournament.Tournament Tournament { get; set; } = null!;
        public ICollection<TournamentPairing.TournamentPairing> Pairings { get; set; } = [];
    }
}
