namespace Application.Tournaments.Responses
{
    public class TournamentParticipantBaseResponse
    {
        public Guid TournamentId { get; set; }
        public Guid UserId { get; set; }

        public required string Status { get; set; }

        public required string DisplayName { get; set; }
        public string? Email { get; set; }
        public string? Notes { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
