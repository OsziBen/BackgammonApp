using Application.GameSessions.Commands.DeclineDoublingCube;
using Application.GameSessions.Realtime;
using Application.Interfaces;
using BackgammonTest.GameSessions.Shared;
using BackgammonTest.TestBuilders;
using Common.Enums.Game;
using Common.Enums.GameSession;
using Domain.GameLogic;
using Domain.GameSession.Results;
using Moq;

namespace BackgammonTest.GameSessions.DeclineDoublingCube
{
    public class DeclineDoublingCubeCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Decline_Cube_Finish_Game_And_Notify()
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

            var boardState = BoardStateBuilder
                .Default()
                .WithOff(offeringPlayer.Color, 15)
                .Build();

            var boardStateFactoryMock = new Mock<IBoardStateFactory>(MockBehavior.Strict);
            boardStateFactoryMock
                .Setup(x => x.Create(session))
                .Returns(boardState);

            var uowMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            uowMock.Setup(x =>
                    x.GameSessions.GetByIdAsync(session.Id, false, false))
                .ReturnsAsync(session);
            uowMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var notifierMock = new Mock<IGameSessionNotifier>(MockBehavior.Strict);
            notifierMock.Setup(x =>
                    x.GameFinished(
                        session.Id,
                        offeringPlayer.Id,
                        GameFinishReason.CubeDeclined,
                        It.IsAny<GameOutcome>()))
                .Returns(Task.CompletedTask);

            var handler = new DeclineDoublingCubeCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                timeProvider,
                boardStateFactoryMock.Object);

            var command = new DeclineDoublingCubeCommand(
                session.Id,
                decliningPlayer.Id);

            // Act
            await handler.Handle(command, default);

            // Assert
            uowMock.Verify(x => x.CommitAsync(), Times.Once);
            boardStateFactoryMock.VerifyAll();
            notifierMock.VerifyAll();
        }
    }
}
