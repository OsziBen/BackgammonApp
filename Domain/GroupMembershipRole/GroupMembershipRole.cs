namespace Domain.GroupMembershipRole
{
    public class GroupMembershipRole
    {
        public Guid GroupMembershipId { get; set; }
        public Guid GroupRoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset AssignedAt { get; set; }
        public Guid GrantedBy { get; set; }
        public DateTimeOffset? RevokedAt { get; set; }

        public GroupMembership.GroupMembership GroupMembership { get; set; } = null!;
        public GroupRole.GroupRole GroupRole { get; set; } = null!;
        public User.User GrantedByUser { get; set; } = null!;
    }
}
