using BackgammonTest.TestBuilders;
using Common.Enums.BoardState;
using Domain.GameLogic;
using Domain.GameLogic.Generators;
using FluentAssertions;

namespace BackgammonTest.Generators
{
    public class BarPriorityTests
    {
        [Theory]
        [InlineData(PlayerColor.White)]
        [InlineData(PlayerColor.Black)]
        public void Must_Enter_From_Bar_First(PlayerColor player)
        {
            // Arange
            var state = BoardStateBuilder.Default()
                .WithCurrentPlayer(player)
                .WithBar(player, 1)
                .WithChecker(6, player)
                .Build();

            var dice = new DiceRoll(new[] { 1, 2 });
            var generator = new MoveSequenceGenerator();

            // Act
            var sequences = generator.Generate(state, dice).ToList();

            // Assert
            sequences.Should().NotBeEmpty();
            sequences.Should().OnlyContain(s => s.Moves[0].From == 0);
        }
    }
}
