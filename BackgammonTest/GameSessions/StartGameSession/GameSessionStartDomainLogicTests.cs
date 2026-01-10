using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GamePlayer;
using Domain.GameSession;
using FluentAssertions;

namespace BackgammonTest.GameSessions.StartGameSession
{
    public class GameSessionStartDomainLogicTests
    {
        private static GameSession CreateValidSession()
        {
            var session = new GameSession
            {
                CurrentPhase = GamePhase.WaitingForPlayers,
                IsFinished = false
            };

            session.Players.Add(
                GamePlayerFactory.CreateHost(session.Id, Guid.NewGuid()));
            session.Players.Add(
                GamePlayerFactory.CreateGuest(session.Id, Guid.NewGuid()));

            return session;
        }

        [Fact]
        public void Start_Should_Set_Phase_And_Timestamps()
        {
            // Arrange
            var session = CreateValidSession();

            // Act
            session.Start();

            // Assert
            session.CurrentPhase.Should()
                .Be(GamePhase.DeterminingStartingPlayer);

            session.StartedAt.Should().NotBeNull();
            session.StartedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
            session.LastUpdatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Start_Should_Throw_When_Session_Is_Finished()
        {
            // Arrange
            var session = CreateValidSession();
            session.IsFinished = true;

            // Act
            Action act = () => session.Start();

            // Assert
            act.Should()
                .Throw<BusinessRuleException>();
        }

        [Fact]
        public void Start_Should_Throw_When_Phase_Is_Not_WaitingForPlayers()
        {
            // Arrange
            var session = CreateValidSession();
            session.CurrentPhase = GamePhase.RollDice;

            // Act
            Action act = () => session.Start();

            // Assert
            act.Should()
                .Throw<BusinessRuleException>();
        }

        [Fact]
        public void Start_Should_Throw_When_Player_Count_Is_Not_Two()
        {
            // Arrange
            var session = new GameSession
            {
                CurrentPhase = GamePhase.WaitingForPlayers
            };

            session.Players.Add(
                GamePlayerFactory.CreateHost(session.Id, Guid.NewGuid()));

            // Act
            Action act = () => session.Start();

            // Assert
            act.Should()
                .Throw<BusinessRuleException>();
        }
    }
}
