using Common.Models;

namespace Domain.GroupMembership
{
    public class GroupMembership : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public DateTimeOffset JoinedAt { get; set; }

        public User.User User { get; set; } = null!;
        public Group.Group Group { get; set; } = null!;
        public ICollection<GroupMembershipRole.GroupMembershipRole> GroupRoles { get; set; } = [];
    }
}
