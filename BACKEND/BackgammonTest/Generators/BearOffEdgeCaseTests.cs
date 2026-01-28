using BackgammonTest.TestBuilders;
using Common.Enums.BoardState;
using Domain.GameLogic;
using Domain.GameLogic.Constants;
using Domain.GameLogic.Generators;
using FluentAssertions;

namespace BackgammonTest.Generators
{
    public class BearOffEdgeCaseTests
    {
        [Theory]
        [InlineData(PlayerColor.White)]
        [InlineData(PlayerColor.Black)]
        public void Cannot_Bear_Off_With_Larger_Die_When_Higher_Checker_Exists(PlayerColor player)
        {
            // Arrange
            var state = BoardStateBuilder.Default()
                .WithCurrentPlayer(player)
                .WithChecker(player == PlayerColor.White ? 13 : 12, player)
                .WithChecker(player == PlayerColor.White ? 24 : 1, player)
                .WithOff(player, 13)
                .Build();

            var dice = new DiceRoll(new[] { 6, 3 });
            var generator = new MoveSequenceGenerator();

            // Act
            var sequences = generator.Generate(state, dice).ToList();

            // Assert
            sequences.Should().NotBeEmpty();
            sequences.SelectMany(s => s.Moves)
                .Should()
                .NotContain(m =>
                    m.Die == 6 &&
                    m.To == BoardConstants.OffBoardPosition);
        }

        [Theory]
        [InlineData(PlayerColor.White)]
        [InlineData(PlayerColor.Black)]
        public void Can_Bear_Off_With_Larger_Die_When_No_Higher_Checker(PlayerColor player)
        {
            // Arrange
            var state = BoardStateBuilder.Default()
                .WithCurrentPlayer(player)
                .WithChecker(player == PlayerColor.White ? 24 : 1, player)
                .WithChecker(player == PlayerColor.White ? 23 : 2, player)
                .WithChecker(player == PlayerColor.White ? 18 : 7, player)
                .WithOff(player, 12)
                .Build();

            var dice = new DiceRoll(new[] { 6, 3 });
            var generator = new MoveSequenceGenerator();

            // Act
            var sequences = generator.Generate(state, dice).ToList();

            // Assert
            sequences.Should().NotBeEmpty();
            sequences.SelectMany(s => s.Moves).Should().Contain(m =>
            m.Die == 6 &&
            m.To == BoardConstants.OffBoardPosition);
        }
    }
}
