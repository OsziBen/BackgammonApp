using Common.Models;

namespace Domain.Comment
{
    public class Comment : BaseEntity
    {
        public Guid AuthorId { get; set; }
        public Guid PostId { get; set; }
        public Guid? ParentCommentId { get; set; }
        public required string Content { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public User.User Author { get; set; } = null!;
        public Post.Post Post { get; set; } = null!;
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = [];
    }
}
