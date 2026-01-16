using Application.GameSessions.Commands.PlayerForfeit;
using Application.GameSessions.Realtime;
using Application.Interfaces;
using BackgammonTest.GameSessions.Shared;
using BackgammonTest.TestBuilders;
using Common.Enums;
using Common.Enums.Game;
using Common.Enums.GameSession;
using Common.Exceptions;
using FluentAssertions;
using Moq;
using System.Numerics;
using System.Reflection;

namespace BackgammonTest.GameSessions.PlayerForfeit
{
    public class PlayerForfeitCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Forfeit_Game_And_Notify()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.MoveCheckers,
                fixedNow);

            var forfeitingPlayer = session.Players.First();
            var winner = session.Players.First(p => p.Id != forfeitingPlayer.Id);

            var boardState = BoardStateBuilder.Default().Build();

            var boardFactoryMock = new Mock<IBoardStateFactory>();
            boardFactoryMock
                .Setup(x => x.Create(session))
                .Returns(boardState);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x =>
                    x.GameSessions.GetByIdAsync(
                        session.Id,
                        false,
                        false))
                .ReturnsAsync(session);

            uowMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var notifierMock = new Mock<IGameSessionNotifier>();
            notifierMock.Setup(x =>
                x.GameFinished(
                    session.Id,
                    winner.Id,
                    GameFinishReason.Forfeit,
                    GameResultType.SimpleVictory))
                .Returns(Task.CompletedTask);

            var handler = new PlayerForfeitCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                dateTimeProvider,
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
                    It.IsAny<GameResultType>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Player_Not_In_Session()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.MoveCheckers,
                dateTimeProvider.UtcNow);

            var winner = session.Players.First();

            var outsiderId = Guid.NewGuid();

            var boardFactoryMock = new Mock<IBoardStateFactory>();
            boardFactoryMock
                .Setup(x => x.Create(session))
                .Returns(BoardStateBuilder.Default().Build());

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x =>
                x.GameSessions.GetByIdAsync(
                    session.Id,
                    false,
                    false))
                .ReturnsAsync(session);

            var notifierMock = new Mock<IGameSessionNotifier>();
            notifierMock.Setup(x =>
                x.GameFinished(
                    session.Id,
                    winner.Id,
                    GameFinishReason.Forfeit,
                    GameResultType.SimpleVictory))
                .Returns(Task.CompletedTask);

            var handler = new PlayerForfeitCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                dateTimeProvider,
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
