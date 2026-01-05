using BackgammonTest.TestBuilders;
using Common.Enums.BoardState;
using Domain.GameLogic;
using Domain.GameLogic.Generators;
using FluentAssertions;

namespace BackgammonTest.Generators
{
    public class EnterFromBarTests
    {
        [Theory]
        [InlineData(PlayerColor.White)]
        [InlineData(PlayerColor.Black)]
        public void Can_Enter_When_Single_Enemy_Checker(PlayerColor player)
        {
            // Arrange
            var state = BoardStateBuilder.Default()
                .WithCurrentPlayer(player)
                .WithBar(player, 1)
                .WithEnemyChecker(player == PlayerColor.White ? 1 : 24, player)
                .WithEnemyChecker(player == PlayerColor.White ? 2 : 23, player, 2)
                .Build();

            var dice = new DiceRoll(new[] { 1, 2 });
            var generator = new MoveSequenceGenerator();
            var expectedPosition = player == PlayerColor.White ? 1 : 24;

            // Act
            var sequences = generator.Generate(state, dice).ToList();

            // Assert
            sequences.SelectMany(s => s.Moves)
                .Should().Contain(m => m.To == expectedPosition);
        }

        [Theory]
        [InlineData(PlayerColor.White)]
        [InlineData(PlayerColor.Black)]
        public void Cannot_Enter_When_Point_Is_Blocked(PlayerColor player)
        {
            // Arrange
            var state = BoardStateBuilder.Default()
                .WithCurrentPlayer(player)
                .WithBar(player, 1)
                .WithEnemyChecker(player == PlayerColor.White ? 1 : 24, player, 2)
                .WithEnemyChecker(player == PlayerColor.White ? 2 : 23, player, 2)
                .Build();

            var dice = new DiceRoll(new[] { 1, 2 });
            var generator = new MoveSequenceGenerator();

            // Act
            var sequences = generator.Generate(state, dice).ToList();

            // Assert
            sequences.Single().Moves.Should().BeEmpty();
        }
    }
}
