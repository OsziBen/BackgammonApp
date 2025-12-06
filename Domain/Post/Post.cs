using Common.Enums.Post;
using Common.Models;

namespace Domain.Post
{
    public class Post : BaseEntity
    {
        public Guid AuthorId { get; set; }
        public Guid GroupId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public PostType Type { get; set; }
        public PostVisibilityType VisibilityType { get; set; }
        public bool IsPinned { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset? PinnedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public User.User Author { get; set; } = null!;
        public Group.Group Group { get; set; } = null!;
        public ICollection<Comment.Comment> Comments { get; set; } = [];
    }
}
