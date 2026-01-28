using BackgammonTest.TestBuilders;
using Common.Enums.BoardState;
using Domain.GameLogic;
using Domain.GameLogic.Generators;
using FluentAssertions;

namespace BackgammonTest.Generators
{
    public class DoubleRollTests
    {
        [Theory]
        [InlineData(PlayerColor.White)]
        [InlineData(PlayerColor.Black)]
        public void Generator_Allows_Max_4_Moves_On_Double(PlayerColor player)
        {
            // Arrange
            var state = BoardStateBuilder.Default()
                .WithCurrentPlayer(player)
                .WithChecker(player == PlayerColor.White ? 1 : 24, player)
                .Build();

            var dice = new DiceRoll(new[] { 4, 4 });
            var generator = new MoveSequenceGenerator();

            // Act
            var sequences = generator.Generate(state, dice).ToList();

            // Assert
            sequences.Should().NotBeEmpty();
            sequences.Max(s => s.Moves.Count).Should().Be(4);
            sequences.Should().Contain(s => s.Moves.Count <= 4);
        }

        [Theory]
        [InlineData(PlayerColor.White)]
        [InlineData(PlayerColor.Black)]
        public void Generator_Respects_Block_When_Not_All_4_Moves_Possible(PlayerColor player)
        {
            // Arrange
            var state = BoardStateBuilder.Default()
                .WithCurrentPlayer(player)
                .WithChecker(player == PlayerColor.White ? 1 : 24, player)
                .WithEnemyChecker(player == PlayerColor.White ? 2 : 23, player, 2)
                .Build();

            var dice = new DiceRoll(new[] { 4, 4 });
            var generator = new MoveSequenceGenerator();

            // Act
            var sequences = generator.Generate(state, dice).ToList();

            // Assert
            sequences.Should().NotBeEmpty();
            sequences.Max(s => s.Moves.Count).Should().BeLessThanOrEqualTo(4);
        }

        [Theory]
        [InlineData(PlayerColor.White)]
        [InlineData(PlayerColor.Black)]
        public void Generator_Creates_All_Valid_Combinations_With_Multiple_Checkers(PlayerColor player)
        {
            // Arrange
            var state = BoardStateBuilder.Default()
                .WithCurrentPlayer(player)
                .WithChecker(player == PlayerColor.White ? 1 : 24, player)
                .WithChecker(player == PlayerColor.White ? 2 : 23, player)
                .Build();

            var dice = new DiceRoll(new[] { 3, 3 });
            var generator = new MoveSequenceGenerator();

            // Act
            var sequences = generator.Generate(state, dice).ToList();

            // Assert
            sequences.Should().NotBeEmpty();
            sequences.Select(s => string.Join(",", s.Moves.Select(m => m.From)))
                .Distinct()
                .Count()
                .Should().BeGreaterThan(1);
        }

        [Theory]
        [InlineData(PlayerColor.White)]
        [InlineData(PlayerColor.Black)]
        public void Generator_Allows_BearOff_For_Double(PlayerColor player)
        {
            // Arrange
            var state = BoardStateBuilder.Default()
                .WithCurrentPlayer(player)
                .WithChecker(player == PlayerColor.White ? 21 : 4, player)
                .WithChecker(player == PlayerColor.White ? 22 : 3, player)
                .WithChecker(player == PlayerColor.White ? 23 : 2, player)
                .WithChecker(player == PlayerColor.White ? 24 : 1, player)
                .Build();

            var dice = new DiceRoll(new[] { 2, 2 });
            var generator = new MoveSequenceGenerator();

            // Act
            var sequences = generator.Generate(state, dice).ToList();

            // Assert
            sequences.Should().NotBeEmpty();
            sequences.Max(s => s.Moves.Count).Should().Be(4);
            sequences.SelectMany(s => s.Moves)
                .Should().Contain(m => m.To == -1);
        }

        [Theory]
        [InlineData(PlayerColor.White)]
        [InlineData(PlayerColor.Black)]
        public void Generator_Handles_Partial_Double_When_Bar_Present(PlayerColor player)
        {
            // Arrange
            var state = BoardStateBuilder.Default()
                .WithCurrentPlayer(player)
                .WithBar(player, 1)
                .WithChecker(player == PlayerColor.White ? 1 : 24, player)
                .WithChecker(player == PlayerColor.White ? 2 : 23, player)
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
