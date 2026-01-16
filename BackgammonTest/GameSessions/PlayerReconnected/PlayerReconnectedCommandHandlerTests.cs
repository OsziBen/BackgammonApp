using Application.GameSessions.Commands.PlayerReconnected;
using Application.GameSessions.Realtime;
using Application.Interfaces;
using BackgammonTest.GameSessions.Shared;
using Domain.GamePlayer;
using FluentAssertions;
using Moq;

namespace BackgammonTest.GameSessions.PlayerReconnected
{
    public class PlayerReconnectedCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Set_IsConnected_True_And_Call_Notifier()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 5, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var player = new GamePlayer
            {
                Id = Guid.NewGuid(),
                GameSessionId = Guid.NewGuid(),
                IsConnected = false
            };

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x =>
                x.GamePlayers.GetByIdAsync(
                    player.Id,
                    false,
                    false))
                .ReturnsAsync(player);

            uowMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var notifierMock = new Mock<IGameSessionNotifier>();
            notifierMock.Setup(x =>
                x.PlayerReconnected(
                    player.GameSessionId,
                    player.Id,
                    It.IsAny<DateTimeOffset>()))
                .Returns(Task.CompletedTask);

            var handler = new PlayerReconnectedCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                dateTimeProvider);

            var command = new PlayerReconnectedCommand(player.Id);

            // Act
            await handler.Handle(command, default);

            // Assert
            player.IsConnected.Should().BeTrue();
            player.LastConnectedAt.Should().Be(fixedNow);

            notifierMock.Verify(x =>
                x.PlayerReconnected(
                    player.GameSessionId,
                    player.Id,
                    It.IsAny<DateTimeOffset>()),
                Times.Once);

            uowMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Do_Nothing_When_Player_AlreadyConnected()
        {
            // Arrange
            var dateTimeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var player = new GamePlayer
            {
                Id = Guid.NewGuid(),
                GameSessionId = Guid.NewGuid(),
                IsConnected = true
            };

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x =>
                x.GamePlayers.GetByIdAsync(
                    player.Id,
                    false,
                    false))
                .ReturnsAsync(player);

            var notifierMock = new Mock<IGameSessionNotifier>();
            notifierMock.Setup(x =>
                x.PlayerReconnected(
                    player.GameSessionId,
                    player.Id,
                    It.IsAny<DateTimeOffset>()))
                .Returns(Task.CompletedTask);

            var handler = new PlayerReconnectedCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                dateTimeProvider);

            var command = new PlayerReconnectedCommand(player.Id);

            // Act
            await handler.Handle(command, default);

            // Assert
            notifierMock.VerifyNoOtherCalls();

            uowMock.Verify(x => x.CommitAsync(), Times.Never);
        }
    }
}
