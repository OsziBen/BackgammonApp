using Application.GameSessions.Commands.CreateGameSession;
using Application.GameSessions.Services.SessionCodeGenerator;
using Application.Interfaces;
using BackgammonTest.GameSessions.Shared;
using Domain.GameSession;
using FluentAssertions;
using Moq;

namespace BackgammonTest.GameSessions.CreateGameSession
{
    public class CreateGameSessionCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Create_Session_And_Return_Session_Id()
        {
            // Arrange
            var dateTimeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var uowMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IGameSessionRepository>();

            uowMock.Setup(x => x.GameSessions)
                .Returns(repoMock.Object);

            uowMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var handler = new CreateGameSessionCommandHandler(
                uowMock.Object,
                new FakeSessionCodeGenerator(),
                dateTimeProvider
            );

            var command = new CreateGameSessionCommand
            (
                Guid.NewGuid(),
                new GameSessionSettings
                {
                    TargerPoints = 3,
                    DoublingCubeEnabled = true
                }
            );

            // Act
            var sessionId = await handler.Handle(command, default);

            // Assert
            sessionId.Should().NotBe(Guid.Empty);

            repoMock.Verify(
                x => x.AddAsync(It.IsAny<GameSession>()),
                Times.Once);

            uowMock.Verify(
                x => x.CommitAsync(),
                Times.Once
);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Player_Has_Active_Session()
        {
            // Arrange
            var dateTimeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var uowMock = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IGameSessionRepository>();

            uowMock.Setup(x => x.GameSessions)
                .Returns(repoMock.Object);

            repoMock.Setup(x => x.HasActiveSession(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var handler = new CreateGameSessionCommandHandler(
                uowMock.Object,
                new FakeSessionCodeGenerator(),
                dateTimeProvider);

            var command = new CreateGameSessionCommand(
                Guid.NewGuid(),
                new GameSessionSettings
                {
                    TargerPoints = 3,
                    DoublingCubeEnabled = true
                });

            // Act
            Func<Task> act = () => handler.Handle(command, default);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>();
        }
    }
}
