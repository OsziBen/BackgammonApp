namespace Application.GameSessions.Responses
{
    public class CreateGameSessionResponse
    {
        public Guid SessionId { get; set; }
        public required string SessionCode { get; set; }
    }
}
