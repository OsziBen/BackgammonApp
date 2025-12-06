using Common.Models;

namespace Domain.GroupRole
{
    public class GroupRole : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public Guid? GroupId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public Group.Group? Group { get; set; }
        public ICollection<GroupMembershipRole.GroupMembershipRole> GroupMembershipRoles { get; set; } = [];
    }
}
