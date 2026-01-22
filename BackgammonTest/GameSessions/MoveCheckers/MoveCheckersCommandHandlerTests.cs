using Application.GameSessions.Commands.MoveCheckers;
using Application.GameSessions.Realtime;
using Application.GameSessions.Requests;
using Application.Interfaces;
using BackgammonTest.GameSessions.Shared;
using BackgammonTest.TestBuilders;
using Common.Enums;
using Common.Enums.BoardState;
using Common.Enums.Game;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GameLogic;
using Domain.GameLogic.Generators;
using Domain.GameSession.Results;
using FluentAssertions;
using Moq;

namespace BackgammonTest.GameSessions.MoveCheckers
{
    public class MoveCheckersCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Throw_When_Not_In_MoveCheckers_Phase()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.RollDice,
                dateTimeProvider.UtcNow);

            var currentPlayer = session.Players.First();
            session.CurrentPlayerId = currentPlayer.Id;

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x =>
                    x.GameSessions.GetByIdAsync(
                        session.Id,
                        false,
                        false))
                .ReturnsAsync(session);

            var handler = new MoveCheckersCommandHandler(
                uowMock.Object,
                Mock.Of<IBoardStateFactory>(),
                Mock.Of<IMoveSequenceGenerator>(),
                Mock.Of<IGameSessionNotifier>(),
                dateTimeProvider);

            var command = new MoveCheckersCommand(
                session.Id,
                currentPlayer.Id,
                new[] { new MoveDto(1, 7, 6) });

            // Act
            Func<Task> act = () => handler.Handle(command, default);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.InvalidGamePhase);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_No_Dice_Rolled()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var timeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.MoveCheckers,
                timeProvider.UtcNow);

            var currentPlayer = session.Players.First();
            session.CurrentPlayerId = currentPlayer.Id;
            session.LastDiceRoll = null;

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x =>
                    x.GameSessions.GetByIdAsync(
                        session.Id,
                        false,
                        false))
                .ReturnsAsync(session);

            var handler = new MoveCheckersCommandHandler(
                uowMock.Object,
                Mock.Of<IBoardStateFactory>(),
                Mock.Of<IMoveSequenceGenerator>(),
                Mock.Of<IGameSessionNotifier>(),
                timeProvider);

            var command = new MoveCheckersCommand(
                session.Id,
                currentPlayer.Id,
                new[] { new MoveDto(1, 7, 6) });

            // Act
            Func<Task> act = () => handler.Handle(command, default);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.InvalidGameState);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Move_Sequence_Is_Invalid()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var timeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.MoveCheckers,
                timeProvider.UtcNow);

            var currentPlayer = session.Players.First();
            session.CurrentPlayerId = currentPlayer.Id;
            session.LastDiceRoll = new[] { 6, 3 };

            var boardState = new BoardState(
                new Dictionary<int, CheckerPosition>(),
                0, 0, 0, 0,
                PlayerColor.White);

            var boardFactoryMock = new Mock<IBoardStateFactory>();
            boardFactoryMock.Setup(x => x.Create(session))
                .Returns(boardState);

            var sequenceGeneratorMock = new Mock<IMoveSequenceGenerator>();
            sequenceGeneratorMock.Setup(x =>
                    x.Generate(boardState, It.IsAny<DiceRoll>()))
                .Returns(Array.Empty<MoveSequence>());

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x =>
                    x.GameSessions.GetByIdAsync(
                        session.Id,
                        false,
                        false))
                .ReturnsAsync(session);

            var notifierMock = new Mock<IGameSessionNotifier>();

            var handler = new MoveCheckersCommandHandler(
                uowMock.Object,
                boardFactoryMock.Object,
                sequenceGeneratorMock.Object,
                notifierMock.Object,
                timeProvider);

            var command = new MoveCheckersCommand(
                session.Id,
                currentPlayer.Id,
                new[] { new MoveDto(1, 7, 6) });

            // Act
            Func<Task> act = () => handler.Handle(command, default);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.InvalidMove);
        }

        [Fact]
        public async Task Handle_Should_Apply_Move_And_End_Turn()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var timeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.MoveCheckers,
                timeProvider.UtcNow);

            var currentPlayer = session.Players.First(p => p.IsHost);
            var nextPlayer = session.Players.First(p => !p.IsHost);

            session.CurrentPlayerId = currentPlayer.Id;
            session.LastDiceRoll = new[] { 6, 3 };

            var boardState = BoardStateBuilder
                .Default()
                .WithCurrentPlayer(currentPlayer.Color)
                .WithChecker(1, currentPlayer.Color, 1)
                .Build();

            var boardFactoryMock = new Mock<IBoardStateFactory>();
            boardFactoryMock
                .Setup(x => x.Create(session))
                .Returns(boardState);

            var sequenceGeneratorMock = new Mock<IMoveSequenceGenerator>();
            sequenceGeneratorMock
                .Setup(x => x.Generate(boardState, It.IsAny<DiceRoll>()))
                .Returns(new[]
                {
            new MoveSequence(new[]
            {
                new Move(1, 7, 6)
            })
                });

            var uowMock = new Mock<IUnitOfWork>();
            uowMock
                .Setup(x => x.GameSessions.GetByIdAsync(
                    session.Id,
                    false,
                    false))
                .ReturnsAsync(session);

            uowMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var notifierMock = new Mock<IGameSessionNotifier>();

            var handler = new MoveCheckersCommandHandler(
                uowMock.Object,
                boardFactoryMock.Object,
                sequenceGeneratorMock.Object,
                notifierMock.Object,
                timeProvider);

            var command = new MoveCheckersCommand(
                session.Id,
                currentPlayer.Id,
                new[] { new MoveDto(1, 7, 6) });

            // Act
            await handler.Handle(command, default);

            // Assert
            uowMock.Verify(x => x.CommitAsync(), Times.Once);

            notifierMock.Verify(x =>
                x.CheckersMoved(
                    session.Id,
                    currentPlayer.Id,
                    command.Moves),
                Times.Once);

            session.CurrentPlayerId.Should().Be(nextPlayer.Id);
            session.LastDiceRoll.Should().BeNull();
            session.IsFinished.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Should_Finish_Game_When_Game_Over()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var timeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.MoveCheckers,
                timeProvider.UtcNow);

            var currentPlayer = session.Players.First(p => p.IsHost);
            var enemyPlayer = session.Players.First(p => !p.IsHost);

            session.CurrentPlayerId = currentPlayer.Id;
            session.LastDiceRoll = new[] { 6, 3 };

            var boardState = BoardStateBuilder
                .Default()
                .WithCurrentPlayer(currentPlayer.Color)
                .WithChecker(24, currentPlayer.Color, 1)
                .WithOff(currentPlayer.Color, 14)
                .WithOff(enemyPlayer.Color, 1)
                .Build();

            var boardFactoryMock = new Mock<IBoardStateFactory>();
            boardFactoryMock
                .Setup(x => x.Create(session))
                .Returns(boardState);

            var sequenceGeneratorMock = new Mock<IMoveSequenceGenerator>();
            sequenceGeneratorMock
                .Setup(x => x.Generate(boardState, It.IsAny<DiceRoll>()))
                .Returns(new[]
                {
                    new MoveSequence(new[]
                    {
                        new Move(24, -1, 6)
                    })
                });

            var uowMock = new Mock<IUnitOfWork>();
            uowMock
                .Setup(x => x.GameSessions.GetByIdAsync(
                    session.Id,
                    false,
                    false))
                .ReturnsAsync(session);

            uowMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var notifierMock = new Mock<IGameSessionNotifier>();

            var handler = new MoveCheckersCommandHandler(
                uowMock.Object,
                boardFactoryMock.Object,
                sequenceGeneratorMock.Object,
                notifierMock.Object,
                timeProvider);

            var command = new MoveCheckersCommand(
                session.Id,
                currentPlayer.Id,
                new[] { new MoveDto(24, -1, 6) });

            // Act
            await handler.Handle(command, default);

            // Assert
            session.IsFinished.Should().BeTrue();
            session.WinnerPlayerId.Should().Be(currentPlayer.Id);

            uowMock.Verify(x => x.CommitAsync(), Times.Once);

            notifierMock.Verify(x =>
                x.GameFinished(
                    session.Id,
                    currentPlayer.Id,
                    GameFinishReason.Victory,
                    new GameOutcome(GameResultType.SimpleVictory, 1)),
                Times.Once);
        }
    }
}
