namespace Application.Tournaments.Requests
{
    public class UpdateTournamentRequest
    {
        public string Name { get; set; } = null!;
        public required string Description { get; set; }
        public required string Type { get; set; }
        public required string Visibility { get; set; }
        public required string Status { get; set; }
        public int MaxParticipants { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset Deadline { get; set; }

        public Guid RulesTemplateId { get; set; }
    }
}
