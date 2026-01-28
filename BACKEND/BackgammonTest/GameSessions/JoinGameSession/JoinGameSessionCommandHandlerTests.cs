using Application.GameSessions.Commands.JoinGameSession;
using Application.GameSessions.Commands.StartGameSession;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GamePlayer;
using Application.Interfaces.Repository.GameSession;
using BackgammonTest.GameSessions.Shared;
using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GamePlayer;
using FluentAssertions;
using MediatR;
using Moq;

namespace BackgammonTest.GameSessions.JoinGameSession
{
    public class JoinGameSessionCommandHandlerTests
    {
        private static (
            Mock<IUnitOfWork>,
            Mock<IGameSessionReadRepository>,
            Mock<IGamePlayerWriteRepository>,
            Mock<IMediator>
            ) CreateMocks()
        {
            var uowMock = new Mock<IUnitOfWork>();
            var sessionReadRepoMock = new Mock<IGameSessionReadRepository>();
            var playerWriteRepoMock = new Mock<IGamePlayerWriteRepository>();
            var mediatorMock = new Mock<IMediator>();

            uowMock
                .Setup(x => x.GamePlayersWrite)
                .Returns(playerWriteRepoMock.Object);

            return (uowMock, sessionReadRepoMock, playerWriteRepoMock, mediatorMock);
        }

        private static JoinGameSessionCommandHandler CreateHandler(
            Mock<IUnitOfWork> uowMock,
            Mock<IMediator> mediatorMock,
            FakedateTimeProvider dateTimeProvider,
            Mock<IGameSessionReadRepository> sessionReadRepoMock)
        {
            return new JoinGameSessionCommandHandler(
                uowMock.Object,
                mediatorMock.Object,
                dateTimeProvider,
                sessionReadRepoMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Join_As_First_Player_When_Session_Is_Empty()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.WaitingForPlayers,
                timeProvider.UtcNow);

            var userId = Guid.NewGuid();

            var (uowMock, sessionRepoMock, playerRepoMock, mediatorMock) = CreateMocks();

            sessionRepoMock
                .Setup(x => x.GetBySessionCodeAsync(session.SessionCode))
                .ReturnsAsync(session);

            uowMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var handler = CreateHandler(
                uowMock,
                mediatorMock,
                timeProvider,
                sessionRepoMock);

            var command = new JoinGameSessionCommand(
                session.SessionCode,
                userId,
                "conn-1");

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            session.Players.Should().HaveCount(1);

            var player = session.Players.First();
            player.UserId.Should().Be(userId);
            player.IsHost.Should().BeTrue();

            result.IsRejoin.Should().BeFalse();

            playerRepoMock.Verify(
                x => x.AddAsync(It.IsAny<GamePlayer>()),
                Times.Once);

            mediatorMock.Verify(
                x => x.Send(It.IsAny<StartGameSessionCommand>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Join_As_Second_Player_And_Start_Game()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.WaitingForPlayers,
                timeProvider.UtcNow);

            session.Players.Add(new GamePlayer
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsHost = true
            });

            var secondUserId = Guid.NewGuid();

            var (uowMock, sessionRepoMock, playerRepoMock, mediatorMock) = CreateMocks();

            sessionRepoMock
                .Setup(x => x.GetBySessionCodeAsync(session.SessionCode))
                .ReturnsAsync(session);

            uowMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var handler = CreateHandler(
                uowMock,
                mediatorMock,
                timeProvider,
                sessionRepoMock);

            var command = new JoinGameSessionCommand(
                session.SessionCode,
                secondUserId,
                "conn-2");

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.IsRejoin.Should().BeFalse();

            playerRepoMock.Verify(
                x => x.AddAsync(It.Is<GamePlayer>(
                    p => p.UserId == secondUserId)),
                Times.Once);

            mediatorMock.Verify(
                x => x.Send(
                    It.Is<StartGameSessionCommand>(c => c.SessionId == session.Id),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Rejoin_When_Player_Already_Exists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existingPlayer = new GamePlayer
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsConnected = false
            };

            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.WaitingForPlayers,
                timeProvider.UtcNow);

            session.Players.Add(existingPlayer);

            var (uowMock, sessionRepoMock, playerRepoMock, mediatorMock) = CreateMocks();

            sessionRepoMock
                .Setup(x => x.GetBySessionCodeAsync(session.SessionCode))
                .ReturnsAsync(session);

            uowMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var handler = CreateHandler(
                uowMock,
                mediatorMock,
                timeProvider,
                sessionRepoMock);

            var command = new JoinGameSessionCommand(
                session.SessionCode,
                userId,
                "conn-rejoin");

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.IsRejoin.Should().BeTrue();

            existingPlayer.IsConnected.Should().BeTrue();
            existingPlayer.LastConnectedAt.Should().NotBeNull();

            playerRepoMock.Verify(
                x => x.Update(existingPlayer),
                Times.Once);

            playerRepoMock.Verify(
                x => x.AddAsync(It.IsAny<GamePlayer>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Session_Is_Full()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateEmptySession(
                GamePhase.WaitingForPlayers,
                timeProvider.UtcNow);

            session.Players.Add(new GamePlayer { UserId = Guid.NewGuid() });
            session.Players.Add(new GamePlayer { UserId = Guid.NewGuid() });

            var (uowMock, sessionRepoMock, playerRepoMock, mediatorMock) = CreateMocks();

            sessionRepoMock
                .Setup(x => x.GetBySessionCodeAsync(session.SessionCode))
                .ReturnsAsync(session);

            uowMock
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var handler = CreateHandler(
                uowMock,
                mediatorMock,
                timeProvider,
                sessionRepoMock);

            var command = new JoinGameSessionCommand(
                session.SessionCode,
                Guid.NewGuid(),
                "conn-full");

            // Act
            Func<Task> act = () => handler.Handle(command, default);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.SessionFull);
        }
    }
}
