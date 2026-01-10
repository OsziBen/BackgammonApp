using Application.GameSessions.Commands.DetermineStartingPlayer;
using Application.GameSessions.Commands.StartGameSession;
using Application.Interfaces;
using Common.Enums.GameSession;
using Domain.GamePlayer;
using Domain.GameSession;
using FluentAssertions;
using MediatR;
using Moq;

namespace BackgammonTest.GameSessions.StartGameSession
{
    public class StartGameSessionCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Start_Session_And_Send_DetermineStartingPlayer_Command()
        {
            // Arrange
            var session = new GameSession
            {
                CurrentPhase = GamePhase.WaitingForPlayers
            };

            session.Players.Add(
                GamePlayerFactory.CreateHost(session.Id, Guid.NewGuid()));
            session.Players.Add(
                GamePlayerFactory.CreateGuest(session.Id, Guid.NewGuid()));

            var uowMock = new Mock<IUnitOfWork>();
            var mediatorMock = new Mock<IMediator>();

            uowMock.Setup(x =>
                x.GameSessions.GetByIdAsync(
                    session.Id,
                    false,
                    false))
                .ReturnsAsync(session);

            uowMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            mediatorMock.Setup(x =>
                x.Send(
                    It.IsAny<DetermineStartingPlayerCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var handler = new StartGameSessionCommandHandler(
                uowMock.Object,
                mediatorMock.Object);

            var command = new StartGameSessionCommand(session.Id);

            // Act
            await handler.Handle(command, default);

            // Assert
            session.CurrentPhase.Should()
                .Be(GamePhase.DeterminingStartingPlayer);

            uowMock.Verify(
                x => x.CommitAsync(),
                Times.Once);

            mediatorMock.Verify(
                x => x.Send(
                    It.Is<DetermineStartingPlayerCommand>(
                        c => c.SessionId == session.Id),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
