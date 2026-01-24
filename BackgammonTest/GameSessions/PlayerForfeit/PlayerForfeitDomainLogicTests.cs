using BackgammonTest.GameSessions.Shared;
using BackgammonTest.TestBuilders;
using Common.Enums;
using Common.Enums.Game;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GameSession.Results;
using FluentAssertions;

namespace BackgammonTest.GameSessions.PlayerForfeit
{
    public class PlayerForfeitDomainLogicTests
    {
        [Fact]
        public void Forfeit_Should_Finish_Game_And_Return_Result()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.MoveCheckers,
                timeProvider.UtcNow);

            var forfeitingPlayer = session.Players.First();
            var winner = session.Players.First(p => p.Id != forfeitingPlayer.Id);

            var boardState = BoardStateBuilder
                .Default()
                .WithOff(winner.Color, 14)
                .Build();

            // Act
            var result = session.Forfeit(
                forfeitingPlayer.Id,
                boardState, timeProvider.UtcNow);

            // Assert
            session.IsFinished.Should().BeTrue();
            session.WinnerPlayerId.Should().Be(winner.Id);

            result.Should().BeEquivalentTo(new GameOutcome(
                GameResultType.GammonVictory,
                2
            ));
        }

        [Fact]
        public void Forfeit_Should_Throw_When_Game_Already_Finished()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.GameFinished,
                timeProvider.UtcNow);

            var playerId = session.Players.First().Id;

            // Act
            Action act = () => session.Forfeit(
                playerId,
                BoardStateBuilder.Default().Build(),
                timeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.GameAlreadyFinished);
        }
    }
}
