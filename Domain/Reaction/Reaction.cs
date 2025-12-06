using Common.Enums.Reaction;
using Common.Models;

namespace Domain.Reaction
{
    public class Reaction : BaseEntity
    {
        public Guid AuthorId { get; set; }
        public Guid TargetId { get; set; }
        public ReactionType Type { get; set; }
        public ReactionTargetType TargetType { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public User.User Author { get; set; } = null!;
    }
}
