using Application.GameSessions.Services.SessionCodeGenerator;

namespace BackgammonTest.GameSessions.Shared
{
    public class FakeSessionCodeGenerator : ISessionCodeGenerator
    {
        public string Generate()
            => "TEST23";
    }
}
