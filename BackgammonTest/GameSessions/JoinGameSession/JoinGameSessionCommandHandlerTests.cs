using Application.GameSessions.Commands.JoinGameSession;
using Application.GameSessions.Commands.StartGameSession;
using Application.Interfaces;
using BackgammonTest.GameSessions.Shared;
using Common.Exceptions;
using Domain.GamePlayer;
using Domain.GameSession;
using FluentAssertions;
using MediatR;
using Moq;

namespace BackgammonTest.GameSessions.JoinGameSession
{
    public class JoinGameSessionCommandHandlerTests
    {
        private static GameSession CreateEmptySession(DateTimeOffset now)
        {
            return new GameSession
            {
                Id = Guid.NewGuid(),
                SessionCode = "ABC123",
                CurrentPhase = Common.Enums.GameSession.GamePhase.WaitingForPlayers,
                Players = new List<GamePlayer>(),
                CreatedAt = now,
                LastUpdatedAt = now
            };
        }

        private static JoinGameSessionCommandHandler CreateHandler(
            Mock<IUnitOfWork> uowMock,
            Mock<IMediator> mediatorMock,
            FakedateTimeProvider dateTimeProvider)
        {
            uowMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            return new JoinGameSessionCommandHandler(
                uowMock.Object,
                mediatorMock.Object,
                dateTimeProvider);
        }

        private static (Mock<IUnitOfWork> uowMock, Mock<IGameSessionRepository> sessionRepoMock, Mock<IGamePlayerRepository> playerRepoMock, Mock<IMediator> mediatorMock) CreateMocks()
        {
            var uowMock = new Mock<IUnitOfWork>();
            var sessionRepoMock = new Mock<IGameSessionRepository>();
            var playerRepoMock = new Mock<IGamePlayerRepository>();
            var mediatorMock = new Mock<IMediator>();

            uowMock.Setup(x => x.GameSessions).Returns(sessionRepoMock.Object);
            uowMock.Setup(x => x.GamePlayers).Returns(playerRepoMock.Object);

            return (uowMock, sessionRepoMock, playerRepoMock, mediatorMock);
        }

        [Fact]
        public async Task Handle_Should_Join_As_First_Player_When_Session_Is_Empty()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = CreateEmptySession(dateTimeProvider.UtcNow);
            var userId = Guid.NewGuid();

            var (uowMock, sessionRepoMock, playerRepoMock, mediatorMock) = CreateMocks();

            sessionRepoMock
                .Setup(x => x.GetBySessionCodeAsync(
                    session.SessionCode,
                    true,
                    false))
                .ReturnsAsync(session);

            var handler = CreateHandler(uowMock, mediatorMock, dateTimeProvider);

            var command = new JoinGameSessionCommand(
                session.SessionCode,
                userId,
                "conn-1");

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            session.Players.Should().HaveCount(1);

            var player = session.Players.Single();
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
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = CreateEmptySession(dateTimeProvider.UtcNow);
            session.Players.Add(new GamePlayer
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsHost = true
            });

            var secondUserId = Guid.NewGuid();

            var (uowMock, sessionRepoMock, playerRepoMock, mediatorMock) = CreateMocks();

            sessionRepoMock
                .Setup(x => x.GetBySessionCodeAsync(
                    session.SessionCode,
                    true,
                    false))
                .ReturnsAsync(session);

            var handler = CreateHandler(uowMock, mediatorMock, dateTimeProvider);

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

            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = CreateEmptySession(dateTimeProvider.UtcNow);
            session.Players.Add(existingPlayer);

            var (uowMock, sessionRepoMock, playerRepoMock, mediatorMock) = CreateMocks();

            sessionRepoMock
                .Setup(x => x.GetBySessionCodeAsync(
                    session.SessionCode,
                    true,
                    false))
                .ReturnsAsync(session);

            var handler = CreateHandler(uowMock, mediatorMock, dateTimeProvider);

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
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var dateTimeProvider = new FakedateTimeProvider(fixedNow);

            var session = CreateEmptySession(dateTimeProvider.UtcNow);

            session.Players.Add(new GamePlayer { UserId = Guid.NewGuid() });
            session.Players.Add(new GamePlayer { UserId = Guid.NewGuid() });

            var (uowMock, sessionRepoMock, playerRepoMock, mediatorMock) = CreateMocks();

            sessionRepoMock
                .Setup(x => x.GetBySessionCodeAsync(
                    session.SessionCode,
                    true,
                    false))
                .ReturnsAsync(session);

            var handler = CreateHandler(uowMock, mediatorMock, dateTimeProvider);

            var command = new JoinGameSessionCommand(
                session.SessionCode,
                Guid.NewGuid(),
                "conn-full");

            // Act
            Func<Task> act = () => handler.Handle(command, default);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Session is full");
        }
    }
}
