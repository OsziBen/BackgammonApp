namespace Application.Users.Responses
{
    public class UserBaseResponse
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required DateTimeOffset JoinedAt { get; set; }
        public int Rating { get; set; }
        public int ExperiencePoints { get; set; }
        public string? GroupRoleName { get; set; }
        public DateTimeOffset? AssignedAt { get; set; }
        public required string? GrantedByUserName { get; set; }
    }
}
