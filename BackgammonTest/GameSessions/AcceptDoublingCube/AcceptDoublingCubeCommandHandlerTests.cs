using Application.GameSessions.Commands.AcceptDoublingCube;
using Application.GameSessions.Realtime;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GameSession;
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
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.CubeOffered,
                timeProvider.UtcNow);

            var offeringPlayer = session.Players.First();
            var acceptingPlayer = session.Players.First(p => p.Id != offeringPlayer.Id);

            session.CurrentPlayerId = offeringPlayer.Id;
            session.DoublingCubeValue = 2;
            session.DoublingCubeOwnerPlayerId = offeringPlayer.Id;

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
