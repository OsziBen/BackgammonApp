namespace Application.Groups.Responses
{
    public class BaseGroupResponse
    {
        public Guid Id { get; set; }
        public required string CreatorName { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        public required string Visibility { get; set; }
        public required string JoinPolicy { get; set; }
        public required string SizePreset { get; set; }

        public int MaxMembers { get; set; }
        public int MaxModerators { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
