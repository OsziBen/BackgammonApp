using Application.GameSessions.Services.SessionCodeGenerator;
using FluentAssertions;

namespace BackgammonTest.GameSessions.CreateGameSession
{
    public class SessionCodeGeneratorTests
    {
        [Fact]
        public void Generate_Should_Return_6_Character_Code()
        {
            // Arrange
            var generator = new SessionCodeGenerator();

            // Act
            var code = generator.Generate();

            // Assert
            code.Should().NotBeNull();
            code.Should().HaveLength(6);
        }

        [Fact]
        public void Generate_Should_Only_Use_Allowed_Characters()
        {
            // Arrange
            var generator = new SessionCodeGenerator();
            const string alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

            // Act
            var code = generator.Generate();

            // Assert
            code.All(ch => alphabet.Contains(ch))
                .Should().BeTrue();
        }

        [Fact]
        public void Generate_Should_Produce_Different_Codes_On_Multiple_Calls()
        {
            // Arrange
            var generator = new SessionCodeGenerator();

            // Act
            var code1 = generator.Generate();
            var code2 = generator.Generate();

            // Assert
            code1.Should().NotBe(code2);
        }
    }
}
