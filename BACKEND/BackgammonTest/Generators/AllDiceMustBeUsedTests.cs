using BackgammonTest.TestBuilders;
using Common.Enums.BoardState;
using Domain.GameLogic;
using Domain.GameLogic.Generators;
using FluentAssertions;

namespace BackgammonTest.Generators
{
    public class AllDiceMustBeUsedTests
    {
        [Theory]
        [InlineData(PlayerColor.White)]
        [InlineData(PlayerColor.Black)]
        public void Uses_Both_Dice_When_Possible(PlayerColor player)
        {
            // Arrange
            var state = BoardStateBuilder.Default()
                .WithCurrentPlayer(player)
                .WithChecker(player == PlayerColor.White ? 1 : 24, player)
                .Build();

            var dice = new DiceRoll(new[] { 1, 2 });
            var generator = new MoveSequenceGenerator();

            // Act
            var sequences = generator.Generate(state, dice).ToList();

            // Assert
            sequences.Should().OnlyContain(s => s.Moves.Count == 2);
        }
    }
}
