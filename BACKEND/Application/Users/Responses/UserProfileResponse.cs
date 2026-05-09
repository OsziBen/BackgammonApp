namespace Application.Users.Responses
{
    public class UserProfileResponse
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public required string EmailAddress { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public int Rating { get; set; }
        public int ExperiencePoints { get; set; }
        public DateTimeOffset CreatedAt { get; set; }   // registration time
    }
}
