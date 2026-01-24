using Application.GameSessions.Commands.PlayerForfeit;
using Application.GameSessions.Realtime;
using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GameSession;
using BackgammonTest.GameSessions.Shared;
using BackgammonTest.TestBuilders;
using Common.Enums;
using Common.Enums.Game;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GameSession.Results;
using FluentAssertions;
using Moq;

namespace BackgammonTest.GameSessions.PlayerForfeit
{
    public class PlayerForfeitCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Forfeit_Game_And_Notify()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.MoveCheckers,
                timeProvider.UtcNow);

            var forfeitingPlayer = session.Players.First();
            var winner = session.Players.First(p => p.Id != forfeitingPlayer.Id);

            var boardState = BoardStateBuilder.Default().Build();

            var boardFactoryMock = new Mock<IBoardStateFactory>();
            boardFactoryMock
                .Setup(x => x.Create(session))
                .Returns(boardState);

            var gameSessionRepoMock =
                new Mock<IGameSessionWriteRepository>();

            gameSessionRepoMock
                .Setup(x => x.GetByIdAsync(session.Id))
                .ReturnsAsync(session);

            var uowMock = new Mock<IUnitOfWork>();

            uowMock
                .Setup(x => x.GameSessionsWrite)
                .Returns(gameSessionRepoMock.Object);

            uowMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var notifierMock = new Mock<IGameSessionNotifier>();
            notifierMock.Setup(x =>
                x.GameFinished(
                    session.Id,
                    winner.Id,
                    GameFinishReason.Forfeit,
                    new GameOutcome(GameResultType.SimpleVictory, 1)))
                .Returns(Task.CompletedTask);

            var handler = new PlayerForfeitCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                timeProvider,
                boardFactoryMock.Object);

            var command = new PlayerForfeitCommand(
                session.Id,
                forfeitingPlayer.Id);

            // Act
            await handler.Handle(command, default);

            // Assert
            session.IsFinished.Should().BeTrue();
            session.WinnerPlayerId.Should().Be(winner.Id);

            uowMock.Verify(x => x.CommitAsync(), Times.Once);

            notifierMock.Verify(x =>
                x.GameFinished(
                    session.Id,
                    winner.Id,
                    GameFinishReason.Forfeit,
                    It.IsAny<GameOutcome>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Player_Not_In_Session()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.MoveCheckers,
                timeProvider.UtcNow);

            var winner = session.Players.First();

            var outsiderId = Guid.NewGuid();

            var boardFactoryMock = new Mock<IBoardStateFactory>();
            boardFactoryMock
                .Setup(x => x.Create(session))
                .Returns(BoardStateBuilder.Default().Build());

            var gameSessionRepoMock =
                new Mock<IGameSessionWriteRepository>();

            gameSessionRepoMock
                .Setup(x => x.GetByIdAsync(session.Id))
                .ReturnsAsync(session);

            var uowMock = new Mock<IUnitOfWork>();

            uowMock
                .Setup(x => x.GameSessionsWrite)
                .Returns(gameSessionRepoMock.Object);

            uowMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var notifierMock = new Mock<IGameSessionNotifier>();
            notifierMock.Setup(x =>
                x.GameFinished(
                    session.Id,
                    winner.Id,
                    GameFinishReason.Forfeit,
                    new GameOutcome(GameResultType.SimpleVictory, 1)))
                .Returns(Task.CompletedTask);

            var handler = new PlayerForfeitCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                timeProvider,
                boardFactoryMock.Object);

            var command = new PlayerForfeitCommand(
                session.Id,
                outsiderId);

            // Act
            Func<Task> act = () => handler.Handle(command, default);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.PlayerNotInSession);
        }
    }
}
