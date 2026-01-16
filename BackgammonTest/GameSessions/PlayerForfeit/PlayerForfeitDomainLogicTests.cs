using BackgammonTest.GameSessions.Shared;
using BackgammonTest.TestBuilders;
using Common.Enums;
using Common.Enums.Game;
using Common.Enums.GameSession;
using Common.Exceptions;
using FluentAssertions;

namespace BackgammonTest.GameSessions.PlayerForfeit
{
    public class PlayerForfeitDomainLogicTests
    {
        [Fact]
        public void Forfeit_Should_Finish_Game_And_Return_Result()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.MoveCheckers,
                dateTimeProvider.UtcNow);

            var forfeitingPlayer = session.Players.First();
            var winner = session.Players.First(p => p.Id != forfeitingPlayer.Id);

            var boardState = BoardStateBuilder
                .Default()
                .WithOff(winner.Color, 14)
                .Build();

            // Act
            var result = session.Forfeit(
                forfeitingPlayer.Id,
                boardState, dateTimeProvider.UtcNow);

            // Assert
            session.IsFinished.Should().BeTrue();
            session.WinnerPlayerId.Should().Be(winner.Id);

            result.Should().Be(GameResultType.SimpleVictory);
        }

        [Fact]
        public void Forfeit_Should_Throw_When_Game_Already_Finished()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.GameFinished,
                dateTimeProvider.UtcNow);

            //session.IsFinished = true;

            var playerId = session.Players.First().Id;

            // Act
            Action act = () => session.Forfeit(
                playerId,
                BoardStateBuilder.Default().Build(),
                dateTimeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.GameAlreadyFinished);
        }
    }
}
