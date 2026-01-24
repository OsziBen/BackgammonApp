using BackgammonTest.GameSessions.Shared;
using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using FluentAssertions;

namespace BackgammonTest.GameSessions.OfferDoublingCube
{
    public class OfferDoublingCubeDomainLogicTests
    {
        [Fact]
        public void OfferDoublingCube_Should_Set_CubeOffered_Phase_And_Update_Timestamp()
        {
            // Arrange
            var fixedNow = new DateTimeOffset(2025, 1, 10, 12, 0, 0, TimeSpan.Zero);
            var timeProvider = new FakedateTimeProvider(fixedNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.TurnStart,
                timeProvider.UtcNow);

            session.CurrentPlayerId = session.Players.First().Id;
            var offeringPlayerId = session.CurrentPlayerId.Value;

            // Act
            var result = session.OfferDoublingCube(
                offeringPlayerId,
                timeProvider.UtcNow);

            // Assert
            session.CurrentPhase.Should().Be(GamePhase.CubeOffered);
            session.LastUpdatedAt.Should().Be(fixedNow);

            result.OfferingPlayerId.Should().Be(offeringPlayerId);
            result.OfferedCubeValue.Should().Be(session.DoublingCubeValue * 2);
        }

        [Fact]
        public void OfferDoublingCube_Should_Throw_When_Not_TurnStart_Phase()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.MoveCheckers,
                timeProvider.UtcNow);

            session.CurrentPlayerId = session.Players.First().Id;

            // Act
            var act = () => session.OfferDoublingCube(
                session.CurrentPlayerId.Value,
                timeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.InvalidGamePhase);
        }

        [Fact]
        public void OfferDoublingCube_Should_Throw_When_Player_Is_Not_Current_Player()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.TurnStart,
                timeProvider.UtcNow);

            session.CurrentPlayerId = session.Players.First().Id;

            var notCurrentPlayerId = session.Players
                .First(p => p.Id != session.CurrentPlayerId)
                .Id;

            // Act
            var act = () => session.OfferDoublingCube(
                notCurrentPlayerId,
                timeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.NotYourTurn);
        }


        [Fact]
        public void OfferDoublingCube_Should_Throw_When_Game_Is_Already_Finished()
        {
            // Arrange
            var timeProvider = new FakedateTimeProvider(DateTimeOffset.UtcNow);

            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.GameFinished,
                timeProvider.UtcNow);

            // Act
            var act = () => session.OfferDoublingCube(
                Guid.NewGuid(),
                timeProvider.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.GameAlreadyFinished);
        }
    }
}
