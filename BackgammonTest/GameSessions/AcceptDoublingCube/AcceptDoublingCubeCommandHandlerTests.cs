using Application.GameSessions.Commands.AcceptDoublingCube;
using Application.GameSessions.Realtime;
using Application.Interfaces;
using BackgammonTest.GameSessions.Shared;
using Common.Enums.GameSession;
using Moq;

namespace BackgammonTest.GameSessions.AcceptDoublingCube
{
    public class AcceptDoublingCubeCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Accept_Cube_Commit_And_Notify()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var timeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.CubeOffered,
                timeProvider.UtcNow);

            var offeringPlayer = session.Players.First();
            var acceptingPlayer = session.Players.First(p => p.Id != offeringPlayer.Id);

            session.CurrentPlayerId = offeringPlayer.Id;
            session.DoublingCubeValue = 2;
            session.DoublingCubeOwnerPlayerId = offeringPlayer.Id;

            var uowMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            uowMock.Setup(x =>
                    x.GameSessions.GetByIdAsync(session.Id, false, false))
                .ReturnsAsync(session);
            uowMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var notifierMock = new Mock<IGameSessionNotifier>(MockBehavior.Strict);
            notifierMock.Setup(x =>
                    x.DoublingCubeAccepted(
                        session.Id,
                        acceptingPlayer.Id,
                        4))
                .Returns(Task.CompletedTask);

            var handler = new AcceptDoublingCubeCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                timeProvider);

            var command = new AcceptDoublingCubeCommand(
                session.Id,
                acceptingPlayer.Id);

            // Act
            await handler.Handle(command, default);

            // Assert
            uowMock.Verify(x => x.CommitAsync(), Times.Once);
            notifierMock.VerifyAll();
        }
    }
}
