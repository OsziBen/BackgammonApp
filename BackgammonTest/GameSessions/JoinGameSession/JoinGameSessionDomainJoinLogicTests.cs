using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GamePlayer;
using Domain.GameSession;
using FluentAssertions;

namespace BackgammonTest.GameSessions.JoinGameSession
{
    public class JoinGameSessionDomainJoinLogicTests
    {
        private static GameSession CreateSession(
            GamePhase phase = GamePhase.WaitingForPlayers,
            bool isFinished = false)
        {
            return new GameSession
            {
                Id = Guid.NewGuid(),
                SessionCode = "ABC123",
                CurrentPhase = phase,
                IsFinished = isFinished,
                Players = new List<GamePlayer>()
            };
        }

        [Fact]
        public void Join_Player_Should_Add_First_Player_As_Host()
        {
            // Arrange
            var session = CreateSession();
            var userId = Guid.NewGuid();

            // Act
            var result = session.JoinPlayer(userId);

            // Assert
            result.IsRejoin.Should().BeFalse();
            result.Player.UserId.Should().Be(userId);
            result.Player.IsHost.Should().BeTrue();

            session.Players.Should().HaveCount(1);
        }

        [Fact]
        public void Join_Player_Should_Add_Second_Player_As_NonHost()
        {
            // Arrange
            var session = CreateSession();
            session.JoinPlayer(Guid.NewGuid());

            var secondUserId = Guid.NewGuid();

            // Act
            var result = session.JoinPlayer(secondUserId);

            // Assert
            result.IsRejoin.Should().BeFalse();
            result.Player.IsHost.Should().BeFalse();

            session.Players.Should().HaveCount(2);
        }

        [Fact]
        public void Join_Player_Should_Rejoin_Existing_Player()
        {
            // Arrange
            var session = CreateSession();
            var userId = Guid.NewGuid();

            var firstJoin = session.JoinPlayer(userId);
            firstJoin.Player.IsConnected = false;

            // Act
            var rejoin = session.JoinPlayer(userId);

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
            var session = CreateSession();
            session.JoinPlayer(Guid.NewGuid());
            session.JoinPlayer(Guid.NewGuid());

            // Act
            Action act = () => session.JoinPlayer(Guid.NewGuid());

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .WithMessage("Session is full");
        }

        [Fact]
        public void Join_Player_Should_Throw_When_Session_Is_Finished()
        {
            // Arrange
            var session = CreateSession(isFinished: true);

            // Act
            Action act = () => session.JoinPlayer(Guid.NewGuid());

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .WithMessage("Cannot join a finished session.");
        }

        [Fact]
        public void Join_Player_Should_Throw_When_Not_Waiting_For_Players()
        {
            // Arrange
            var session = CreateSession(phase: GamePhase.RollDice);

            // Act
            Action act = () => session.JoinPlayer(Guid.NewGuid());

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .WithMessage("Cannot join session in phase RollDice.");
        }

    }
}
