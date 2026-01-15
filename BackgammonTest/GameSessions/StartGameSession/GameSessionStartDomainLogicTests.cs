using BackgammonTest.GameSessions.Shared;
using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GamePlayer;
using FluentAssertions;

namespace BackgammonTest.GameSessions.StartGameSession
{
    public class GameSessionStartDomainLogicTests
    {
        [Fact]
        public void Start_Should_Set_Phase_And_Timestamps()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.WaitingForPlayers,
                dateTimeProvider.UtcNow);

            // Act
            session.Start(dateTimeProvider.UtcNow);

            // Assert
            session.CurrentPhase.Should()
                .Be(GamePhase.DeterminingStartingPlayer);

            session.StartedAt.Should().NotBeNull();
            session.StartedAt.Should().Be(fixedNow);
            session.LastUpdatedAt.Should().Be(fixedNow);
        }

        [Fact]
        public void Start_Should_Throw_When_Session_Is_Finished()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.GameFinished,
                dateTimeProvider.UtcNow);

            // Act
            Action act = () => session.Start(dateTimeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.GameAlreadyFinished); ;
        }

        [Fact]
        public void Start_Should_Throw_When_Phase_Is_Not_WaitingForPlayers()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.RollDice,
                dateTimeProvider.UtcNow);

            // Act
            Action act = () => session.Start(dateTimeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>();
        }

        [Fact]
        public void Start_Should_Throw_When_Player_Count_Is_Not_Two()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.WaitingForPlayers,
                dateTimeProvider.UtcNow);

            session.Players.Add(
                GamePlayerFactory.CreateHost(
                    session.Id,
                    Guid.NewGuid(),
                    dateTimeProvider.UtcNow)
                );

            // Act
            Action act = () => session.Start(dateTimeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>();
        }
    }
}
