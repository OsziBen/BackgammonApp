using BackgammonTest.TestBuilders;
using Common.Enums.BoardState;
using Domain.GameLogic;
using Domain.GameLogic.Generators;
using FluentAssertions;

namespace BackgammonTest.Generators
{
    public class PreferHigherDieIntegratedTests
    {

        [Theory]
        [InlineData(PlayerColor.White)]
        [InlineData(PlayerColor.Black)]
        public void Generator_Enforces_Higher_Die_When_Available(PlayerColor player)
        {
            // Arrange
            var state = BoardStateBuilder.Default()
                .WithCurrentPlayer(player)
                .WithChecker(player == PlayerColor.White ? 8 : 16, player)
                .WithEnemyChecker(player == PlayerColor.White ? 16 : 8, player, 2)
                .Build();

            var dice = new DiceRoll(new[] { 3, 5 });
            var generator = new MoveSequenceGenerator();

            // Act
            var sequences = generator.Generate(state, dice).ToList();

            // Assert
            sequences.Should().NotBeEmpty();
            sequences.SelectMany(s => s.Moves).Should().OnlyContain(m => m.Die == 5);
        }
    }
}
