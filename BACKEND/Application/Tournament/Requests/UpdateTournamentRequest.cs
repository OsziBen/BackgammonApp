namespace Application.Tournament.Requests
{
    public class UpdateTournamentRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public required string Status { get; set; }
    }
}
