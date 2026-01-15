using BackgammonTest.GameSessions.Shared;
using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GamePlayer;
using FluentAssertions;

namespace BackgammonTest.GameSessions.DetermineStartingPlayer
{
    public class DetermineStartingPlayerDomainLogicTests
    {
        [Fact]
        public void DetermineStartingPlayer_Should_Set_Starting_Player_And_Phase()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.DeterminingStartingPlayer,
                dateTimeProvider.UtcNow);

            var player1 = session.Players.Single(p => p.IsHost);
            var player2 = session.Players.Single(p => !p.IsHost);

            // Act
            var result = session.DetermineStartingPlayer(
                roll1: 6,
                roll2: 2,
                dateTimeProvider.UtcNow);

            // Assert
            session.CurrentPhase.Should().Be(GamePhase.MoveCheckers);
            session.CurrentPlayerId.Should().Be(player1.Id);
            session.LastUpdatedAt.Should().Be(fixedNow);

            player1.StartingRoll.Should().Be(6);
            player2.StartingRoll.Should().Be(2);

            result.StarttingPlayerId.Should().Be(player1.Id);
            result.Rolls.Should().BeEquivalentTo(new[]
            {
                (player1.Id, 6),
                (player2.Id, 2)
            });
        }

        [Fact]
        public void DetermineStartingPlayer_Should_Throw_When_Same_Dice_Values()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.DeterminingStartingPlayer,
                dateTimeProvider.UtcNow);

            // Act
            var act = () => session.DetermineStartingPlayer(
                5,
                5,
                dateTimeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .WithMessage("Starting rolls must be distinct");
        }

        [Fact]
        public void DetermineStartingPlayer_Should_Throw_When_Wrong_Phase()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.WaitingForPlayers,
                dateTimeProvider.UtcNow);

            // Act
            var act = () => session.DetermineStartingPlayer(
                3,
                5,
                dateTimeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .WithMessage("*Expected phase*");
        }

        [Fact]
        public void DetermineStartingPlayer_Should_Throw_When_Finished()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.GameFinished,
                dateTimeProvider.UtcNow);

            // Act
            var act = () => session.DetermineStartingPlayer(
                4,
                1,
                dateTimeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.GameAlreadyFinished); ;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        public void DetermineStartingPlayer_Should_Throw_When_Player_Count_Invalid(int count)
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.DeterminingStartingPlayer,
                dateTimeProvider.UtcNow);

            for (int i = 0; i < count; i++)
            {
                session.Players.Add(
                    i == 0
                    ? GamePlayerFactory.CreateHost(session.Id, Guid.NewGuid(), dateTimeProvider.UtcNow)
                    : GamePlayerFactory.CreateGuest(session.Id, Guid.NewGuid(), dateTimeProvider.UtcNow)
                );
            }

            // Act
            var act = () => session.DetermineStartingPlayer(
                2,
                5,
                dateTimeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .WithMessage("*Exactly two players*");
        }
    }
}
