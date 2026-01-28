using Application.GameSessions.Commands.OfferDoublingCube;
using Application.GameSessions.Realtime;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.GameSession;
using BackgammonTest.GameSessions.Shared;
using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using FluentAssertions;
using Moq;

namespace BackgammonTest.GameSessions.OfferDoublingCube
{
    public class OfferDoublingCubeCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Offer_Cube_And_Notify()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.TurnStart,
                timeProvider.UtcNow);

            session.CurrentPlayerId = session.Players.First().Id;
            var offeringPlayerId = session.CurrentPlayerId.Value;

            var currentCubeValue = session.DoublingCubeValue!.Value;
            var offeredValue = currentCubeValue * 2;

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
                    x.DoublingCubeOffered(
                        session.Id,
                        offeringPlayerId,
                        offeredValue))
                .Returns(Task.CompletedTask);

            var handler = new OfferDoublingCubeCommandHandler(
                uowMock.Object,
                notifierMock.Object,
                timeProvider);

            var command = new OfferDoublingCubeCommand(
                session.Id,
                offeringPlayerId);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            uowMock.Verify(x => x.CommitAsync(), Times.Once);

            notifierMock.Verify(x =>
                x.DoublingCubeOffered(
                    session.Id,
                    offeringPlayerId,
                    offeredValue),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Domain_Rule_Fails()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.GameFinished,
                timeProvider.UtcNow);

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

            var handler = new OfferDoublingCubeCommandHandler(
                uowMock.Object,
                Mock.Of<IGameSessionNotifier>(),
                timeProvider);

            var command = new OfferDoublingCubeCommand(
                session.Id,
                Guid.NewGuid());

            // Act
            var act = async () => await handler.Handle(command, default);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.GameAlreadyFinished);
        }
    }
}
