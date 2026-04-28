namespace Application.Users.Responses
{
    public class UserGroupJoinRequestResponse
    {
        public Guid Id { get; set; }
        public required string GroupName { get; set; }
        public required string Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
