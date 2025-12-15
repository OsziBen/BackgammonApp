using Common.Enums.TournamentRegistration;
using Common.Models;

namespace Domain.TournamentRegistration
{
    public class TournamentRegistration : BaseEntity
    {
        public Guid TournamentId { get; set; }
        public Guid ParticipantId { get; set; }

        public TournamentRegistrationStatus Status { get; set; }

        public int? Seed { get; set; }      // Swiss / SE első kör

        public DateTimeOffset? ConfirmedAt { get; set; }
        public DateTimeOffset? CancelledAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }


        public Tournament.Tournament Tournament { get; set; } = null!;
        public TournamentParticipant.TournamentParticipant Participant { get; set; } = null!;
    }
}
