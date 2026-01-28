using BackgammonTest.GameSessions.Shared;
using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GameLogic;
using FluentAssertions;
using Moq;

namespace BackgammonTest.GameSessions.RollDice
{
    public class RollDiceDomainLogicTests
    {
        [Fact]
        public void RollDice_Should_Set_LastDiceRoll_And_Change_Phase()
        {
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var timeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.RollDice,
                timeProvider.UtcNow);

            session.CurrentPlayerId = session.Players.First().Id;

            var rollerMock = new Mock<IDiceRoller>();
            rollerMock
                .Setup(x => x.Roll())
                .Returns(new DiceRoll(3, 5));

            // Act
            session.RollDice(
                session.CurrentPlayerId!.Value,
                rollerMock.Object,
                timeProvider.UtcNow);

            // Assert
            session.LastDiceRoll.Should().BeEquivalentTo(new[] { 3, 5 });
            session.CurrentPhase.Should().Be(GamePhase.MoveCheckers);
            session.LastUpdatedAt.Should().Be(fixedNow);
        }

        [Fact]
        public void RollDice_Should_Throw_When_Game_Is_Finished()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.GameFinished,
                timeProvider.UtcNow);

            var rollerMock = new Mock<IDiceRoller>();
            rollerMock
                .Setup(x => x.Roll())
                .Returns(new DiceRoll(3, 5));

            // Act
            var act = () => session.RollDice(
                Guid.NewGuid(),
                rollerMock.Object,
                timeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.GameAlreadyFinished);
        }

        [Fact]
        public void RollDice_Should_Throw_When_Not_In_RollDice_Phase()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.MoveCheckers,
                timeProvider.UtcNow);

            session.CurrentPlayerId = session.Players.First().Id;

            var rollerMock = new Mock<IDiceRoller>();
            rollerMock
                .Setup(x => x.Roll())
                .Returns(new DiceRoll(3, 5));

            // Act
            var act = () => session.RollDice(
                session.CurrentPlayerId!.Value,
                rollerMock.Object,
                timeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.InvalidGamePhase);
        }

        [Fact]
        public void RollDice_Should_Throw_When_Player_Is_Not_Current_Player()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.RollDice,
                timeProvider.UtcNow);

            session.CurrentPlayerId = session.Players.First().Id;

            var rollerMock = new Mock<IDiceRoller>();
            rollerMock
                .Setup(x => x.Roll())
                .Returns(new DiceRoll(3, 5));

            var notCurrentPlayer = session.Players
                .First(p => p.Id != session.CurrentPlayerId!.Value)
                .Id;

            // Act
            var act = () => session.RollDice(
                notCurrentPlayer,
                rollerMock.Object,
                timeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.NotYourTurn);
        }

        [Fact]
        public void RollDice_Should_Throw_When_Dice_Already_Rolled()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.RollDice,
                timeProvider.UtcNow);

            session.CurrentPlayerId = session.Players.First().Id;

            var rollerMock = new Mock<IDiceRoller>();
            rollerMock
                .Setup(x => x.Roll())
                .Returns(new DiceRoll(3, 5));

            session.LastDiceRoll = new[] { 6, 6 };

            // Act
            var act = () => session.RollDice(
                session.CurrentPlayerId!.Value,
                rollerMock.Object,
                timeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.DiceAlreadyRolled);
        }
    }
}
