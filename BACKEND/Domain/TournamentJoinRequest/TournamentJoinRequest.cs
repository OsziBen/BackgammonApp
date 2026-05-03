using Common.Enums.Group;
using Common.Models;

namespace Domain.TournamentJoinRequest
{
    public class TournamentJoinRequest : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid TournamentId { get; set; }

        public JoinRequestStatus Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ReviewedAt { get; set; }
        public Guid? ReviewedByUserId { get; set; }

        public User.User User { get; set; } = null!;
        public Tournament.Tournament Tournament { get; set; } = null!;
        public User.User? ReviewedByUser { get; set; }
    }
}
