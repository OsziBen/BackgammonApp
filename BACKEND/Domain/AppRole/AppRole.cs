using Common.Models;

namespace Domain.AppRole
{
    public class AppRole : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }

        public ICollection<User.User> Users { get; set; } = [];
    }
}
