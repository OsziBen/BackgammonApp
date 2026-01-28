using BackgammonTest.TestBuilders;
using Common.Enums.BoardState;
using Domain.GameLogic;
using Domain.GameLogic.Generators;
using FluentAssertions;

namespace BackgammonTest.Generators
{
    public class MoveSequenceGeneratorTests
    {
        [Theory]
        [InlineData(PlayerColor.White)]
        [InlineData(PlayerColor.Black)]
        public void Generates_No_Moves_When_Blocked(PlayerColor player)
        {
            // Arrange
            var state = BoardStateBuilder.Default()
                .WithCurrentPlayer(player)
                .WithChecker(player == PlayerColor.White ? 1 : 24, player)
                .WithEnemyChecker(player == PlayerColor.White ? 2 : 23, player, 2)
                .WithEnemyChecker(player == PlayerColor.White ? 3 : 22, player, 2)
                .Build();

            var roll = new DiceRoll(new[] { 1, 2 });
            var generator = new MoveSequenceGenerator();

            // Act
            var sequences = generator.Generate(state, roll);

            // Assert
            sequences.Should().ContainSingle();
            sequences[0].Moves.Should().BeEmpty();
        }
    }
}
