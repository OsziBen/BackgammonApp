using BackgammonTest.GameSessions.Shared;
using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GamePlayer;
using Domain.GameSession.Services;
using FluentAssertions;
using Moq;

namespace BackgammonTest.GameSessions.DetermineStartingPlayer
{
    public class DetermineStartingPlayerDomainLogicTests
    {
        [Fact]
        public void DetermineStartingPlayer_Should_Set_Starting_Player_And_Phase()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var timeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.DeterminingStartingPlayer,
                timeProvider.UtcNow);

            var startingPlayerRollerMock = new Mock<IStartingPlayerRoller>();
            startingPlayerRollerMock
                .Setup(x => x.Roll())
                .Returns(new StartingPlayerRoll(6, 3));

            var player1 = session.Players.First(p => p.IsHost);
            var player2 = session.Players.First(p => !p.IsHost);

            // Act
            var result = session.DetermineStartingPlayer(
                startingPlayerRollerMock.Object,
                timeProvider.UtcNow);

            // Assert
            session.CurrentPhase.Should().Be(GamePhase.MoveCheckers);
            session.CurrentPlayerId.Should().Be(player1.Id);
            session.LastUpdatedAt.Should().Be(fixedNow);

            player1.StartingRoll.Should().Be(6);
            player2.StartingRoll.Should().Be(3);

            result.StartingPlayerId.Should().Be(player1.Id);
            result.Rolls.Should().BeEquivalentTo(new[]
            {
                (player1.Id, 6),
                (player2.Id, 3)
            });
        }

        [Fact]
        public void StartingPlayerRoll_Should_Throw_When_Rolls_Are_Equal()
        {
            // Arrange

            // Act
            var act = () => new StartingPlayerRoll(3, 3);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.InvalidDiceRollValues);
        }

        [Fact]
        public void DetermineStartingPlayer_Should_Throw_When_Wrong_Phase()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.WaitingForPlayers,
                timeProvider.UtcNow);

            var startingPlayerRollerMock = new Mock<IStartingPlayerRoller>();
            startingPlayerRollerMock
                .Setup(x => x.Roll())
                .Returns(new StartingPlayerRoll(6, 3));

            // Act
            var act = () => session.DetermineStartingPlayer(
                startingPlayerRollerMock.Object,
                timeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.InvalidGamePhase);
        }

        [Fact]
        public void DetermineStartingPlayer_Should_Throw_When_Finished()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.GameFinished,
                timeProvider.UtcNow);

            var startingPlayerRollerMock = new Mock<IStartingPlayerRoller>();

            // Act
            var act = () => session.DetermineStartingPlayer(
                startingPlayerRollerMock.Object,
                timeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.GameAlreadyFinished);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        public void DetermineStartingPlayer_Should_Throw_When_Player_Count_Invalid(int count)
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.DeterminingStartingPlayer,
                timeProvider.UtcNow);

            var startingPlayerRollerMock = new Mock<IStartingPlayerRoller>();

            for (int i = 0; i < count; i++)
            {
                session.Players.Add(
                    i == 0
                    ? GamePlayerFactory.CreateHost(session.Id, Guid.NewGuid(), timeProvider.UtcNow)
                    : GamePlayerFactory.CreateGuest(session.Id, Guid.NewGuid(), timeProvider.UtcNow)
                );
            }

            // Act
            var act = () => session.DetermineStartingPlayer(
                startingPlayerRollerMock.Object,
                timeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.InsufficientPlayerNumber);
        }
    }
}
