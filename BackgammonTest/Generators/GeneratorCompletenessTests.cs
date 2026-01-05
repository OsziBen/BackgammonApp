using BackgammonTest.TestBuilders;
using Common.Enums.BoardState;
using Domain.GameLogic;
using Domain.GameLogic.Generators;
using FluentAssertions;

namespace BackgammonTest.Generators
{
    public class GeneratorCompletenessTests
    {
        [Theory]
        [InlineData(PlayerColor.White)]
        [InlineData(PlayerColor.Black)]
        public void Finds_All_Valid_Move_Orders(PlayerColor player)
        {
            // Arrange
            var state = BoardStateBuilder.Default()
                .WithCurrentPlayer(player)
                .WithChecker(player == PlayerColor.White ? 1 : 24, player)
                .WithChecker(player == PlayerColor.White ? 2 : 23, player)
                .Build();

            var dice = new DiceRoll(new[] { 1, 2 });
            var generator = new MoveSequenceGenerator();

            // Act
            var sequences = generator.Generate(state, dice).ToList();

            // Assert
            sequences.Should().HaveCountGreaterThan(1);

            sequences.Should().Contain(s =>
                s.Moves[0].Die == 1 && s.Moves[1].Die == 2);

            sequences.Should().Contain(s =>
                s.Moves[0].Die == 2 && s.Moves[1].Die == 1);
        }
    }
}
