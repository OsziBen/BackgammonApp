using BackgammonTest.GameSessions.Shared;
using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using FluentAssertions;

namespace BackgammonTest.GameSessions.AcceptDoublingCube
{
    public class AcceptDoublingCubeDomainLogicTests
    {
        [Fact]
        public void AcceptDoublingCube_Should_Update_Game_State_Correctly()
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

            // Act
            var result = session.AcceptDoublingCube(
                acceptingPlayer.Id,
                timeProvider.UtcNow);

            // Assert
            session.CurrentPhase.Should().Be(GamePhase.RollDice);
            session.CurrentPlayerId.Should().Be(offeringPlayer.Id);

            session.DoublingCubeOwnerPlayerId.Should().Be(acceptingPlayer.Id);
            session.DoublingCubeValue.Should().Be(4);

            result.AcceptingPlayerId.Should().Be(acceptingPlayer.Id);
            result.OfferingPlayerId.Should().Be(offeringPlayer.Id);
            result.NewCubeValue.Should().Be(4);
        }

        [Fact]
        public void AcceptDoublingCube_Should_Throw_When_Game_Is_Finished()
        {
            // Arrange
            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.GameFinished);

            var playerId = Guid.NewGuid();

            // Act
            var act = () => session.AcceptDoublingCube(
                playerId,
                DateTimeOffset.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.GameAlreadyFinished);
        }

        [Fact]
        public void AcceptDoublingCube_Should_Throw_When_Not_In_CubeOffered_Phase()
        {
            // Arrange
            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.RollDice);

            // Act
            var act = () => session.AcceptDoublingCube(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.InvalidGamePhase);
        }

        [Fact]
        public void AcceptDoublingCube_Should_Throw_When_Offering_Player_Tries_To_Accept()
        {
            // Arrange
            var session = TestGameSessionFactory.CreateValidSession(
                GamePhase.CubeOffered);

            var offeringPlayer = session.Players.First();
            session.CurrentPlayerId = offeringPlayer.Id;

            // Act
            var act = () => session.AcceptDoublingCube(
                offeringPlayer.Id,
                DateTimeOffset.UtcNow);

            // Assert
            act.Should()
                .Throw<BusinessRuleException>()
                .Where(e => e.ErrorCode == FunctionCode.InvalidPlayer);
        }

        [Fact]
        public void Accept_Should_Double_Cube_Change_Owner_And_Move_To_RollDice()
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

            // Act
            var result = session.AcceptDoublingCube(
                acceptingPlayer.Id,
                timeProvider.UtcNow);

            // Assert
            session.CurrentPhase.Should().Be(GamePhase.RollDice);
            session.CurrentPlayerId.Should().Be(offeringPlayer.Id);

            session.DoublingCubeValue.Should().Be(4);
            session.DoublingCubeOwnerPlayerId.Should().Be(acceptingPlayer.Id);

            result.NewCubeValue.Should().Be(4);
            result.AcceptingPlayerId.Should().Be(acceptingPlayer.Id);
            result.OfferingPlayerId.Should().Be(offeringPlayer.Id);
        }
    }
}
