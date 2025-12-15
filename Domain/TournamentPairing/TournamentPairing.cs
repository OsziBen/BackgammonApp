using Common.Enums.TournamentPairing;
using Common.Models;

namespace Domain.TournamentPairing
{
    public class TournamentPairing : BaseEntity
    {
        public Guid TournamentRoundId { get; set; }
        public Guid WhiteParticipantId { get; set; }
        public Guid BlackParticipantId { get; set; }

        public TournamentPairingResult? Result { get; set; }
        public string? RecordingUrl { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }


        public TournamentRound.TournamentRound TournamentRound { get; set; } = null!;
        public TournamentParticipant.TournamentParticipant WhiteParticipant { get; set; } = null!;
        public TournamentParticipant.TournamentParticipant BlackParticipant { get; set; } = null!;

    }
}
