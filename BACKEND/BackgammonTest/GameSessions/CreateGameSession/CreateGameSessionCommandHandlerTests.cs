using Application.GameSessions.Commands.CreateGameSession;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GameSession;
using BackgammonTest.GameSessions.Shared;
using Common.Enums;
using Common.Exceptions;
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
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var gameSessionReadRepoMock = new Mock<IGameSessionReadRepository>();
            gameSessionReadRepoMock
                .Setup(x => x.HasActiveSession(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var gameSessionWriteRepoMock = new Mock<IGameSessionWriteRepository>();

            var uowMock = new Mock<IUnitOfWork>();
            uowMock
                .Setup(x => x.GameSessionsWrite)
                .Returns(gameSessionWriteRepoMock.Object);
            uowMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var handler = new CreateGameSessionCommandHandler(
                uowMock.Object,
                new FakeSessionCodeGenerator(),
                timeProvider,
                gameSessionReadRepoMock.Object
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

            gameSessionWriteRepoMock.Verify(
                x => x.AddAsync(It.IsAny<GameSession>()),
                Times.Once);

            uowMock.Verify(
                x => x.CommitAsync(),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Player_Has_Active_Session()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var gameSessionReadRepoMock = new Mock<IGameSessionReadRepository>();
            gameSessionReadRepoMock
                .Setup(x => x.HasActiveSession(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var gameSessionWriteRepoMock = new Mock<IGameSessionWriteRepository>();

            var uowMock = new Mock<IUnitOfWork>();
            uowMock
                .Setup(x => x.GameSessionsWrite)
                .Returns(gameSessionWriteRepoMock.Object);
            uowMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var handler = new CreateGameSessionCommandHandler(
                uowMock.Object,
                new FakeSessionCodeGenerator(),
                timeProvider,
                gameSessionReadRepoMock.Object);

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
                .ThrowAsync<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.SessionAlreadyStarted);
        }
    }
}
