using Common.Enums.Group;
using Common.Models;

namespace Domain.GroupJoinRequest
{
    public class GroupJoinRequest : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }

        public JoinRequestStatus Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ReviewedAt { get; set; }
        public Guid? ReviewedByUserId { get; set; }

        public User.User User { get; set; } = null!;
        public Group.Group Group { get; set; } = null!;
        public User.User? ReviewedByUser { get; set; }
    }
}
