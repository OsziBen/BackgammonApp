using BackgammonTest.GameSessions.Shared;
using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using FluentAssertions;

namespace BackgammonTest.GameSessions.JoinGameSession
{
    public class JoinGameSessionDomainJoinLogicTests
    {
        [Fact]
        public void Join_Player_Should_Add_First_Player_As_Host()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.WaitingForPlayers,
                timeProvider.UtcNow);

            var userId = Guid.NewGuid();

            // Act
            var result = session.JoinPlayer(userId, timeProvider.UtcNow);

            // Assert
            result.IsRejoin.Should().BeFalse();
            result.Player.UserId.Should().Be(userId);
            result.Player.IsHost.Should().BeTrue();

            session.Players.Should().HaveCount(1);
            session.Players.First(p => p.IsHost).UserId.Should().Be(userId);
        }

        [Fact]
        public void Join_Player_Should_Add_Second_Player_As_NonHost()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.WaitingForPlayers,
                timeProvider.UtcNow);

            session.JoinPlayer(Guid.NewGuid(), timeProvider.UtcNow);

            var secondUserId = Guid.NewGuid();

            // Act
            var result = session.JoinPlayer(secondUserId, timeProvider.UtcNow);

            // Assert
            result.IsRejoin.Should().BeFalse();
            result.Player.IsHost.Should().BeFalse();

            session.Players.Should().HaveCount(2);
            session.Players.First(p => !p.IsHost).UserId.Should().Be(secondUserId);
        }

        [Fact]
        public void Join_Player_Should_Rejoin_Existing_Player()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.WaitingForPlayers,
                timeProvider.UtcNow);

            var userId = Guid.NewGuid();

            var firstJoin = session.JoinPlayer(userId, timeProvider.UtcNow);
            firstJoin.Player.IsConnected = false;

            // Act
            var rejoin = session.JoinPlayer(userId, timeProvider.UtcNow);

            // Assert
            rejoin.IsRejoin.Should().BeTrue();
            rejoin.Player.IsConnected.Should().BeTrue();
            rejoin.Player.LastConnectedAt.Should().NotBeNull();

            session.Players.Should().HaveCount(1);
        }

        [Fact]
        public void Join_Player_Should_Throw_When_Session_Is_Full()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.WaitingForPlayers,
                timeProvider.UtcNow);

            session.JoinPlayer(Guid.NewGuid(), timeProvider.UtcNow);
            session.JoinPlayer(Guid.NewGuid(), timeProvider.UtcNow);

            // Act
            Action act = () => session.JoinPlayer(Guid.NewGuid(), timeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.SessionFull);
        }

        [Fact]
        public void Join_Player_Should_Throw_When_Session_Is_Finished()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.GameFinished,
                timeProvider.UtcNow);

            // Act
            Action act = () => session.JoinPlayer(Guid.NewGuid(), timeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.GameAlreadyFinished);
        }

        [Fact]
        public void Join_Player_Should_Throw_When_Not_Waiting_For_Players()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.RollDice,
                timeProvider.UtcNow);

            // Act
            Action act = () => session.JoinPlayer(Guid.NewGuid(), timeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.InvalidGamePhase);
        }

    }
}
