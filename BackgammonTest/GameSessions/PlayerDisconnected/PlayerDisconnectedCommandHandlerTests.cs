using Application.GameSessions.Commands.PlayerDisconnected;
using Application.GameSessions.Realtime;
using Application.Interfaces;
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
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

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
                dateTimeProvider);

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
            var dateTimeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x =>
                x.GamePlayers.GetByIdAsync(
                    It.IsAny<Guid>(),
                    false,
                    false))
                .ReturnsAsync(() => null);

            var notifierMock = new Mock<IGameSessionNotifier>();

            var handler = new PlayerDisconnectedCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                dateTimeProvider);

            var command = new PlayerDisconnectedCommand(Guid.NewGuid());

            // Act
            await handler.Handle(command, default);

            // Assert
            notifierMock.VerifyNoOtherCalls();

            uowMock.Verify(x => x.CommitAsync(), Times.Never);
        }
    }
}
