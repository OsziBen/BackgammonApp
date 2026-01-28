using Application.GameSessions.Commands.RollDice;
using Application.GameSessions.Realtime;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GameSession;
using BackgammonTest.GameSessions.Shared;
using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GameLogic;
using Domain.GameSession.Services;
using FluentAssertions;
using Moq;

namespace BackgammonTest.GameSessions.RollDice
{
    public class RollDiceCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Roll_Dice_And_Notify()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.DeterminingStartingPlayer,
                timeProvider.UtcNow);

            var startingPlayerRollerMock = new Mock<IStartingPlayerRoller>();
            startingPlayerRollerMock
                .Setup(x => x.Roll())
                .Returns(new StartingPlayerRoll(6, 3));

            session.DetermineStartingPlayer(
                startingPlayerRollerMock.Object,
                timeProvider.UtcNow);

            session.EndTurn(timeProvider.UtcNow);

            var uowMock = new Mock<IUnitOfWork>();
            var sessionWriteRepoMock = new Mock<IGameSessionWriteRepository>();

            sessionWriteRepoMock
                .Setup(x => x.GetByIdAsync(session.Id))
                .ReturnsAsync(session);

            uowMock
                .Setup(x => x.GameSessionsWrite)
                .Returns(sessionWriteRepoMock.Object);

            uowMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var notifierMock = new Mock<IGameSessionNotifier>();
            notifierMock
                .Setup(x => x.DiceRolled(
                    session.Id,
                    session.CurrentPlayerId!.Value,
                    4,
                    2))
                .Returns(Task.CompletedTask);

            var diceRollerMock = new Mock<IDiceRoller>();
            diceRollerMock
                .Setup(x => x.Roll())
                .Returns(new DiceRoll(4, 2));

            var handler = new RollDiceCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                timeProvider,
                diceRollerMock.Object);

            var command = new RollDiceCommand(
                session.Id,
                session.CurrentPlayerId!.Value);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Die1.Should().Be(4);
            result.Die2.Should().Be(2);
            result.MovesCount.Should().Be(result.Die1 == result.Die2 ? 4 : 2);


            uowMock.Verify(x => x.CommitAsync(), Times.Once);

            notifierMock.Verify(x =>
                x.DiceRolled(
                    session.Id,
                    session.CurrentPlayerId!.Value,
                    4,
                    2),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Game_Is_Finished()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.GameFinished,
                timeProvider.UtcNow);

            var uowMock = new Mock<IUnitOfWork>();
            var gameSessionsWriteMock = new Mock<IGameSessionWriteRepository>();
            gameSessionsWriteMock
                .Setup(x => x.GetByIdAsync(session.Id))
                .ReturnsAsync(session);
            uowMock.Setup(x => x.GameSessionsWrite).Returns(gameSessionsWriteMock.Object);

            var diceRollerMock = new Mock<IDiceRoller>();
            diceRollerMock
                .Setup(x => x.Roll())
                .Returns(new DiceRoll(4, 2));

            var handler = new RollDiceCommandHandler(
                uowMock.Object,
                Mock.Of<IGameSessionNotifier>(),
                timeProvider,
                diceRollerMock.Object);

            var command = new RollDiceCommand(
                session.Id,
                Guid.NewGuid());

            // Act
            var act = async () => await handler.Handle(command, default);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.GameAlreadyFinished);
        }

    }
}
