using Common.Enums.TournamentParticipant;
using Common.Models;

namespace Domain.TournamentParticipant
{
    public class TournamentParticipant : BaseEntity
    {
        public Guid TournamentId { get; set; }
        public Guid? UserId { get; set; }
        public TournamentParticipantStatus Status { get; set; }

        public required string DisplayName { get; set; }
        public string? Email { get; set; }
        public string? Notes { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }


        public Tournament.Tournament Tournament { get; set; } = null!;
        public User.User? User { get; set; }
        public ICollection<TournamentRegistration.TournamentRegistration> Registrations { get; set; } = [];
    }
}
