using Application.GameSessions.Commands.DetermineStartingPlayer;
using Application.GameSessions.Commands.StartGameSession;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GamePlayer;
using Application.Interfaces.Repository.GameSession;
using BackgammonTest.GameSessions.Shared;
using Common.Enums.GameSession;
using FluentAssertions;
using MediatR;
using Moq;

namespace BackgammonTest.GameSessions.StartGameSession
{
    public class StartGameSessionCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Start_Session_And_Send_DetermineStartingPlayer_Command()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var timeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.WaitingForPlayers,
                timeProvider.UtcNow);

            var playerWriteRepoMock = new Mock<IGamePlayerWriteRepository>();

            var gameSessionsWriteMock = new Mock<IGameSessionWriteRepository>();
            gameSessionsWriteMock
            .Setup(x => x.GetByIdAsync(session.Id))
            .ReturnsAsync(session);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock
                .Setup(x => x.GamePlayersWrite)
                .Returns(playerWriteRepoMock.Object);
            uowMock
                .Setup(x => x.GameSessionsWrite)
                .Returns(gameSessionsWriteMock.Object);
            uowMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x =>
                x.Send(
                    It.IsAny<DetermineStartingPlayerCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var handler = new StartGameSessionCommandHandler(
                uowMock.Object,
                mediatorMock.Object,
                timeProvider);

            var command = new StartGameSessionCommand(session.Id);

            // Act
            await handler.Handle(command, default);

            // Assert
            session.CurrentPhase.Should()
                .Be(GamePhase.DeterminingStartingPlayer);

            uowMock.Verify(
                x => x.CommitAsync(),
                Times.Once);

            mediatorMock.Verify(
                x => x.Send(
                    It.Is<DetermineStartingPlayerCommand>(
                        c => c.SessionId == session.Id),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
