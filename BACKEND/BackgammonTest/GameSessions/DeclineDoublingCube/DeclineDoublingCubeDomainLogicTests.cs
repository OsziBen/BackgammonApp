using BackgammonTest.GameSessions.Shared;
using BackgammonTest.TestBuilders;
using Common.Enums.Game;
using Common.Enums.GameSession;
using Domain.GameSession.Results;
using FluentAssertions;

namespace BackgammonTest.GameSessions.DeclineDoublingCube
{
    public class DeclineDoublingCubeDomainLogicTests
    {
        [Fact]
        public void DeclineDoublingCube_Should_Finish_Game_And_Return_Correct_Outcome()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var timeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.CubeOffered,
                timeProvider.UtcNow);

            var offeringPlayer = session.Players.First();
            var decliningPlayer = session.Players.First(p => p.Id != offeringPlayer.Id);

            session.CurrentPlayerId = offeringPlayer.Id;
            session.DoublingCubeValue = 2;

            var boardState = BoardStateBuilder.Default()
                .WithOff(offeringPlayer.Color, 15)
                .Build();

            // Act
            var outcome = session.DeclineDoublingCube(
                decliningPlayer.Id,
                boardState,
                timeProvider.UtcNow);

            // Assert
            session.IsFinished.Should().BeTrue();
            session.CurrentPhase.Should().Be(GamePhase.GameFinished);
            session.WinnerPlayerId.Should().Be(offeringPlayer.Id);

            outcome.Should().BeEquivalentTo(new GameOutcome(
                GameResultType.GammonVictory,
                4));
        }

        [Fact]
        public void Decline_Should_Result_In_SimpleVictory()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var timeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.CubeOffered,
                timeProvider.UtcNow);

            var offeringPlayer = session.Players.First();
            var decliningPlayer = session.Players.First(p => p.Id != offeringPlayer.Id);

            session.CurrentPlayerId = offeringPlayer.Id;
            session.DoublingCubeValue = 2;

            var boardState = BoardStateBuilder.Default()
                .WithOff(offeringPlayer.Color, 2)
                .WithOff(decliningPlayer.Color, 1)
                .Build();

            // Act
            var outcome = session.DeclineDoublingCube(
                decliningPlayer.Id,
                boardState,
                timeProvider.UtcNow);

            // Assert
            outcome.Should().BeEquivalentTo(new GameOutcome(
                GameResultType.SimpleVictory,
                2));
        }

        [Fact]
        public void Decline_Should_Result_In_GammonVictory()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var timeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.CubeOffered,
                timeProvider.UtcNow);

            var offeringPlayer = session.Players.First();
            var decliningPlayer = session.Players.First(p => p.Id != offeringPlayer.Id);

            session.CurrentPlayerId = offeringPlayer.Id;
            session.DoublingCubeValue = 2;

            var boardState = BoardStateBuilder.Default()
                .WithOff(offeringPlayer.Color, 15)
                .Build();

            // Act
            var outcome = session.DeclineDoublingCube(
                decliningPlayer.Id,
                boardState,
                timeProvider.UtcNow);

            // Assert
            outcome.Should().BeEquivalentTo(new GameOutcome(
                GameResultType.GammonVictory,
                4));
        }

        [Fact]
        public void Decline_Should_Result_In_BackgammonVictory()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var timeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.CubeOffered,
                timeProvider.UtcNow);

            var offeringPlayer = session.Players.First();
            var decliningPlayer = session.Players.First(p => p.Id != offeringPlayer.Id);

            session.CurrentPlayerId = offeringPlayer.Id;
            session.DoublingCubeValue = 2;

            var boardState = BoardStateBuilder.Default()
                .WithOff(offeringPlayer.Color, 15)
                .WithBar(decliningPlayer.Color, 1)
                .Build();

            // Act
            var outcome = session.DeclineDoublingCube(
                decliningPlayer.Id,
                boardState,
                timeProvider.UtcNow);

            // Assert
            outcome.Should().BeEquivalentTo(new GameOutcome(
                GameResultType.BackgammonVictory,
                6));
        }
    }
}
