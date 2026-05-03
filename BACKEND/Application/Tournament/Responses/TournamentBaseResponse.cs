namespace Application.Tournament.Responses
{
    public class TournamentBaseResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public required string Type { get; set; }
        public required string Visibility { get; set; }
        public required string Status { get; set; }
        public int MaxParticipants { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset Deadline { get; set; }
        public required string OrganizerUserName { get; set; }
        // TODO: template csak részleteknél!
        // TODO: timestamps saját oldalhoz?
        // TODO: canJoin ide is
    }
}
