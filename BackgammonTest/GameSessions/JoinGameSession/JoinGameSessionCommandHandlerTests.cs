using Application.GameSessions.Commands.JoinGameSession;
using Application.GameSessions.Commands.StartGameSession;
using Application.Interfaces;
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
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IGameSessionRepository> _sessionRepoMock = new();
        private readonly Mock<IGamePlayerRepository> _playerRepoMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();

        private JoinGameSessionCommandHandler CreateHandler()
        {
            _uowMock.Setup(x => x.GameSessions)
                .Returns(_sessionRepoMock.Object);

            _uowMock.Setup(x => x.GamePlayers)
                .Returns(_playerRepoMock.Object);

            _uowMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            return new JoinGameSessionCommandHandler(
                _uowMock.Object,
                _mediatorMock.Object);
        }

        private static GameSession CreateEmptySession()
        {
            return new GameSession
            {
                Id = Guid.NewGuid(),
                SessionCode = "ABC123",
                CurrentPhase = Common.Enums.GameSession.GamePhase.WaitingForPlayers,
                Players = new List<GamePlayer>(),
                CreatedAt = DateTimeOffset.UtcNow,
                LastUpdatedAt = DateTimeOffset.UtcNow
            };
        }

        [Fact]
        public async Task Handle_Should_Join_As_First_Player_When_Session_Is_Empty()
        {
            // Arrange
            var session = CreateEmptySession();
            var userId = Guid.NewGuid();

            _sessionRepoMock
                .Setup(x => x.GetBySessionCodeAsync(
                    session.SessionCode,
                    true,
                    false))
                .ReturnsAsync(session);

            var handler = CreateHandler();

            var command = new JoinGameSessionCommand(
                session.SessionCode,
                userId,
                "conn-1");

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.IsRejoin.Should().BeFalse();

            _playerRepoMock.Verify(
                x => x.AddAsync(It.Is<GamePlayer>(
                    p => p.UserId == userId &&
                    p.IsHost)),
                Times.Once);

            _mediatorMock.Verify(
                x => x.Send(
                    It.IsAny<StartGameSessionCommand>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Join_As_Second_Player_And_Start_Game()
        {
            // Arrange
            var session = CreateEmptySession();

            session.Players.Add(new GamePlayer
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                IsHost = true
            });

            var secondUserId = Guid.NewGuid();

            _sessionRepoMock
                .Setup(x => x.GetBySessionCodeAsync(
                    session.SessionCode,
                    true,
                    false))
                .ReturnsAsync(session);

            var handler = CreateHandler();

            var command = new JoinGameSessionCommand(
                session.SessionCode,
                secondUserId,
                "conn-2");

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.IsRejoin.Should().BeFalse();

            _playerRepoMock.Verify(
                x => x.AddAsync(It.Is<GamePlayer>(
                    p => p.UserId == secondUserId)),
                Times.Once);

            _mediatorMock.Verify(
                x => x.Send(
                    It.Is<StartGameSessionCommand>(c =>
                        c.SessionId == session.Id),
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

            var session = CreateEmptySession();
            session.Players.Add(existingPlayer);

            _sessionRepoMock
                .Setup(x => x.GetBySessionCodeAsync(
                    session.SessionCode,
                    true,
                    false))
                .ReturnsAsync(session);

            var handler = CreateHandler();

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

            _playerRepoMock.Verify(
                x => x.Update(existingPlayer),
                Times.Once);

            _playerRepoMock.Verify(
                x => x.AddAsync(It.IsAny<GamePlayer>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Session_Is_Full()
        {
            // Arrange
            var session = CreateEmptySession();

            session.Players.Add(new GamePlayer { UserId = Guid.NewGuid() });
            session.Players.Add(new GamePlayer { UserId = Guid.NewGuid() });

            _sessionRepoMock
                .Setup(x => x.GetBySessionCodeAsync(
                    session.SessionCode,
                    true,
                    false))
                .ReturnsAsync(session);

            var handler = CreateHandler();

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
