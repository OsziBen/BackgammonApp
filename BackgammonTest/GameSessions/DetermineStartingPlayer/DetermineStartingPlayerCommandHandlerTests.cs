using Application.GameSessions.Commands.DetermineStartingPlayer;
using Application.GameSessions.Realtime;
using Application.Interfaces;
using BackgammonTest.GameSessions.Shared;
using Common.Enums.GameSession;
using Domain.GamePlayer;
using FluentAssertions;
using Moq;

namespace BackgammonTest.GameSessions.DetermineStartingPlayer
{
    public class DetermineStartingPlayerCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Do_Nothing_When_Not_Enough_Players()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.DeterminingStartingPlayer,
                dateTimeProvider.UtcNow);

            session.Players.Add(
                GamePlayerFactory.CreateHost(
                    session.Id,
                    Guid.NewGuid(),
                    dateTimeProvider.UtcNow)
                );

            var playerRepoMock = new Mock<IGamePlayerRepository>();
            var uowMock = new Mock<IUnitOfWork>();
            var diceMock = new Mock<IDiceService>();
            var notifierMock = new Mock<IGameSessionNotifier>();

            playerRepoMock.Setup(x => x.GetPlayersBySessionAsync(session.Id, false))
                .ReturnsAsync(session.Players.ToList());

            uowMock.Setup(x =>
                x.GameSessions.GetByIdAsync(
                    session.Id,
                    false,
                    false))
                .ReturnsAsync(session);

            uowMock.Setup(x => x.GamePlayers)
                .Returns(playerRepoMock.Object);

            uowMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            diceMock.Setup(x => x.RollDistinctPair())
                .Returns((6, 3));

            notifierMock.Setup(x => x.StartingPlayerDetermined(
                    It.IsAny<Guid>(),
                    It.IsAny<(Guid, int)[]>(),
                    It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            var handler = new DetermineStartingPlayerCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                diceMock.Object,
                dateTimeProvider);

            var command = new DetermineStartingPlayerCommand(session.Id);

            // Act
            await handler.Handle(command, default);

            // Assert
            uowMock.Verify(x => x.CommitAsync(), Times.Never);

            diceMock.Verify(x => x.RollDistinctPair(), Times.Never);

            notifierMock.Verify(x =>
                x.StartingPlayerDetermined(
                    It.IsAny<Guid>(),
                    It.IsAny<(Guid, int)[]>(),
                    It.IsAny<Guid>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Determine_Starting_Player_And_Notify()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.DeterminingStartingPlayer,
                dateTimeProvider.UtcNow);

            var player1 = session.Players.Single(p => p.IsHost);
            var player2 = session.Players.Single(p => !p.IsHost);

            var playerRepoMock = new Mock<IGamePlayerRepository>();
            var uowMock = new Mock<IUnitOfWork>();
            var diceMock = new Mock<IDiceService>();
            var notifierMock = new Mock<IGameSessionNotifier>();

            playerRepoMock.Setup(x =>
                x.GetPlayersBySessionAsync(
                    session.Id,
                    false))
                .ReturnsAsync(session.Players.ToList());

            uowMock.Setup(x =>
                x.GameSessions.GetByIdAsync(
                    session.Id,
                    false,
                    false))
                .ReturnsAsync(session);

            uowMock.Setup(x => x.GamePlayers)
                .Returns(playerRepoMock.Object);

            uowMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            diceMock.Setup(x => x.RollDistinctPair())
                .Returns((6, 3));

            notifierMock.Setup(x => x.StartingPlayerDetermined(
                    It.IsAny<Guid>(),
                    It.IsAny<(Guid, int)[]>(),
                    It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            var handler = new DetermineStartingPlayerCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                diceMock.Object,
                dateTimeProvider);

            var command = new DetermineStartingPlayerCommand(session.Id);

            // Act
            await handler.Handle(command, default);

            // Assert
            uowMock.Verify(x => x.CommitAsync(), Times.Once);

            notifierMock.Verify(x =>
                x.StartingPlayerDetermined(
                    session.Id,
                    It.Is<(Guid playerId, int value)[]>(rolls =>
                        rolls.Length == 2 &&
                        rolls.Any(r => r.playerId == player1.Id && r.value == 6) &&
                        rolls.Any(r => r.playerId == player2.Id && r.value == 3)),
                    It.Is<Guid>(id => id == player1.Id)),
                Times.Once);

            session.CurrentPhase.Should().Be(GamePhase.MoveCheckers);
            session.CurrentPlayerId.Should().Be(player1.Id);
        }
    }
}
