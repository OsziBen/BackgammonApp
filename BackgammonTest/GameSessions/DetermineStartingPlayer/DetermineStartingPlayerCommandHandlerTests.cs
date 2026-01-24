using Application.GameSessions.Commands.DetermineStartingPlayer;
using Application.GameSessions.Realtime;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GameSession;
using BackgammonTest.GameSessions.Shared;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GamePlayer;
using Domain.GameSession.Services;
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
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.DeterminingStartingPlayer,
                timeProvider.UtcNow);

            session.Players.Add(
                GamePlayerFactory.CreateHost(
                    session.Id,
                    Guid.NewGuid(),
                    timeProvider.UtcNow)
                );

            var sessionWriteRepoMock = new Mock<IGameSessionWriteRepository>();
            sessionWriteRepoMock
                .Setup(x => x.GetByIdAsync(session.Id))
                .ReturnsAsync(session);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock
                .Setup(x => x.GameSessionsWrite)
                .Returns(sessionWriteRepoMock.Object);
            uowMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var notifierMock = new Mock<IGameSessionNotifier>();
            notifierMock.Setup(x => x.StartingPlayerDetermined(
                    It.IsAny<Guid>(),
                    It.IsAny<(Guid, int)[]>(),
                    It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            var startingPlayerRollerMock = new Mock<IStartingPlayerRoller>();
            startingPlayerRollerMock
                .Setup(x => x.Roll())
                .Returns(new StartingPlayerRoll(6, 3));

            var handler = new DetermineStartingPlayerCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                timeProvider,
                startingPlayerRollerMock.Object);

            var command = new DetermineStartingPlayerCommand(session.Id);

            // Act
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                handler.Handle(command, default));

            // Assert
            uowMock.Verify(x => x.CommitAsync(), Times.Never);

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
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.DeterminingStartingPlayer,
                timeProvider.UtcNow);

            var host = session.Players.First(p => p.IsHost);
            var opponent = session.Players.First(p => !p.IsHost);

            var sessionWriteRepoMock = new Mock<IGameSessionWriteRepository>();
            sessionWriteRepoMock
                .Setup(x => x.GetByIdAsync(session.Id))
                .ReturnsAsync(session);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock
                .Setup(x => x.GameSessionsWrite)
                .Returns(sessionWriteRepoMock.Object);
            uowMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var notifierMock = new Mock<IGameSessionNotifier>();
            notifierMock.Setup(x => x.StartingPlayerDetermined(
                    It.IsAny<Guid>(),
                    It.IsAny<(Guid, int)[]>(),
                    It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            var startingPlayerRollerMock = new Mock<IStartingPlayerRoller>();
            startingPlayerRollerMock
                .Setup(x => x.Roll())
                .Returns(new StartingPlayerRoll(6, 3));

            var handler = new DetermineStartingPlayerCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                timeProvider,
                startingPlayerRollerMock.Object);

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
                        rolls.Any(r => r.playerId == host.Id && r.value == 6) &&
                        rolls.Any(r => r.playerId == opponent.Id && r.value == 3)),
                    It.Is<Guid>(id => id == host.Id)),
                Times.Once);

            session.CurrentPhase.Should().Be(GamePhase.MoveCheckers);
            session.CurrentPlayerId.Should().Be(host.Id);
        }
    }
}
