namespace Application.Groups.Responses
{
    public class GroupJoinRequestResponse
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ReviewedAt { get; set; }
        public string? ReviewedByUser { get; set; }
    }
}
