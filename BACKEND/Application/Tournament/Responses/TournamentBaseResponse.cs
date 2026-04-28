namespace Application.Tournament.Responses
{
    public class TournamentBaseResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public required string Type { get; set; }
        public required string Status { get; set; }
        public DateTimeOffset StartDate { get; set; }
    }
}
