using Common.Models;

namespace Domain.User
{
    public class User : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public required string EmailAddress { get; set; }
        public required string PasswordHash { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required string ProfilePictureUrl { get; set; }
        public Guid AppRoleId { get; set; }
        public int Rating { get; set; }
        public int ExperiencePoints { get; set; }
        public bool IsBanned { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSystemUser { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public AppRole.AppRole AppRole { get; set; } = null!;
        public ICollection<GroupMembership.GroupMembership> GroupMemberships { get; set; } = [];
        public ICollection<Group.Group> CreatedGroups { get; set; } = [];
        public ICollection<GroupMembershipRole.GroupMembershipRole> GrantedRoles { get; set; } = [];
    }
}
