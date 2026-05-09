namespace Application.Users.Responses
{
    public class UserTournamentJoinRequestResponse
    {
        public Guid Id { get; set; }
        public required string TournamentName { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset Deadline { get; set; }
        public required string OrganizerUserName { get; set; }
        public required string Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
