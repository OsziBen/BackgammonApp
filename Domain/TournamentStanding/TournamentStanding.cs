using Common.Models;

namespace Domain.TournamentStanding
{
    public class TournamentStanding : BaseEntity
    {
        public Guid TournamentId { get; set; }
        public Guid ParticipantId { get; set; }

        public int Points { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int ByeCount { get; set; }   // Cached aggregated value, derived from pairings with Result = Bye

        public int? TieBreakScore { get; set; }   // pl. Buchholz, Sonneborn-Berger, stb.

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }


        public Tournament.Tournament Tournament { get; set; } = null!;
        public TournamentParticipant.TournamentParticipant Participant { get; set; } = null!;
    }
}
