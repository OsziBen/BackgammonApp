using Domain.GameLogic;
using Domain.GameLogic.Rules;
using FluentAssertions;

namespace BackgammonTest.Rules
{
    public class PreferHigherDieRuleTests
    {
        [Fact]
        public void Filter_Seqquences_To_Those_Using_Higher_Die()
        {
            // Arrange
            var seqences = new[]
            {
                new MoveSequence(new[]
                {
                    new Move(1, 4, 3)
                }),
                new MoveSequence(new[]
                {
                    new Move(1, 6, 5)
                })
            };

            var rule = new PreferHigherDieRule();
            var roll = new DiceRoll(new[] { 3, 5 });

            // Act
            var result = rule.Apply(seqences, roll).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Moves.First().Die.Should().Be(5);
        }
    }
}
