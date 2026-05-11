namespace Application.Tournaments.Requests
{
    public class CreateTournamentRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public required string Type { get; set; }
        public required string Visibility { get; set; }
        public int MaxParticipants { get; set; }
        public Guid RulesTemplateId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset Deadline { get; set; }
    }
}
