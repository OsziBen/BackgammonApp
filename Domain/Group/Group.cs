using Common.Enums;
using Common.Models;

namespace Domain.Group
{
    public class Group : BaseEntity
    {
        public Guid CreatorId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public GroupType GroupType { get; set; }
        public int MaxMembers { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public User.User Creator { get; set; } = null!;
        public ICollection<GroupMembership.GroupMembership> GroupMemberships { get; set; } = [];
        public ICollection<GroupRole.GroupRole> GroupRoles { get; set; } = [];
    }
}
