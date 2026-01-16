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
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.WaitingForPlayers,
                dateTimeProvider.UtcNow);

            var userId = Guid.NewGuid();

            // Act
            var result = session.JoinPlayer(userId, dateTimeProvider.UtcNow);

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
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.WaitingForPlayers,
                dateTimeProvider.UtcNow);

            session.JoinPlayer(Guid.NewGuid(), dateTimeProvider.UtcNow);

            var secondUserId = Guid.NewGuid();

            // Act
            var result = session.JoinPlayer(secondUserId, dateTimeProvider.UtcNow);

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
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.WaitingForPlayers,
                dateTimeProvider.UtcNow);

            var userId = Guid.NewGuid();

            var firstJoin = session.JoinPlayer(userId, dateTimeProvider.UtcNow);
            firstJoin.Player.IsConnected = false;

            // Act
            var rejoin = session.JoinPlayer(userId, dateTimeProvider.UtcNow);

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
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.WaitingForPlayers,
                dateTimeProvider.UtcNow);

            session.JoinPlayer(Guid.NewGuid(), dateTimeProvider.UtcNow);
            session.JoinPlayer(Guid.NewGuid(), dateTimeProvider.UtcNow);

            // Act
            Action act = () => session.JoinPlayer(Guid.NewGuid(), dateTimeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.SessionFull);
        }

        [Fact]
        public void Join_Player_Should_Throw_When_Session_Is_Finished()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.GameFinished,
                dateTimeProvider.UtcNow);

            // Act
            Action act = () => session.JoinPlayer(Guid.NewGuid(), dateTimeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.GameAlreadyFinished);
        }

        [Fact]
        public void Join_Player_Should_Throw_When_Not_Waiting_For_Players()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.RollDice,
                dateTimeProvider.UtcNow);

            // Act
            Action act = () => session.JoinPlayer(Guid.NewGuid(), dateTimeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.InvalidGamePhase);
        }

    }
}
