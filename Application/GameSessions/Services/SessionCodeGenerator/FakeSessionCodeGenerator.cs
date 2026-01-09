namespace Application.GameSessions.Services.SessionCodeGenerator
{
    public class FakeSessionCodeGenerator : ISessionCodeGenerator
    {
        public string Generate()
            => "TEST01";
    }
}
