using Application.GameSessions.Commands.PlayerDisconnected;
using Application.GameSessions.Realtime;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GamePlayer;
using BackgammonTest.GameSessions.Shared;
using Domain.GamePlayer;
using FluentAssertions;
using Moq;

namespace BackgammonTest.GameSessions.PlayerDisconnected
{
    public class PlayerDisconnectedCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Set_IsConnected_False_And_Call_Notifier()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var timeProvider = new FakedateTimeProvider(fixedNow);

            var player = new GamePlayer
            {
                Id = Guid.NewGuid(),
                GameSessionId = Guid.NewGuid(),
                IsConnected = true
            };

            var playerWriteRepoMock = new Mock<IGamePlayerWriteRepository>();
            playerWriteRepoMock
                .Setup(x => x.GetByIdAsync(player.Id))
                .ReturnsAsync(player);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock
                .Setup(x => x.GamePlayersWrite)
                .Returns(playerWriteRepoMock.Object);

            uowMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var notifierMock = new Mock<IGameSessionNotifier>();
            notifierMock.Setup(x => x.PlayerDisconnected(
                    player.GameSessionId,
                    player.Id,
                    It.IsAny<DateTimeOffset>()))
                .Returns(Task.CompletedTask);

            var handler = new PlayerDisconnectedCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                timeProvider);

            var command = new PlayerDisconnectedCommand(player.Id);

            // Act
            await handler.Handle(command, default);

            // Assert
            player.IsConnected.Should().BeFalse();
            player.LastConnectedAt.Should().Be(fixedNow);

            notifierMock.Verify(x =>
                x.PlayerDisconnected(
                    player.GameSessionId,
                    player.Id,
                    fixedNow),
                Times.Once);

            uowMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Do_Nothing_When_Player_NotFound()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var playerWriteRepoMock = new Mock<IGamePlayerWriteRepository>();

            var uowMock = new Mock<IUnitOfWork>();
            uowMock
                .Setup(x => x.GamePlayersWrite)
                .Returns(playerWriteRepoMock.Object);

            uowMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var notifierMock = new Mock<IGameSessionNotifier>();

            var handler = new PlayerDisconnectedCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                timeProvider);

            var command = new PlayerDisconnectedCommand(Guid.NewGuid());

            // Act
            await handler.Handle(command, default);

            // Assert
            notifierMock.VerifyNoOtherCalls();

            uowMock.Verify(x => x.CommitAsync(), Times.Never);
        }
    }
}
