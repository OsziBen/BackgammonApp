using Domain.GameLogic;
using Domain.GameLogic.Rules;
using FluentAssertions;

namespace BackgammonTest.Rules
{
    public class MustUseMaxDiceRuleTests
    {
        [Fact]
        public void Keep_Only_Sequences_Using_Maximum_Number_Of_Moves()
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
                    new Move(1, 4, 3),
                    new Move(4, 9, 5)
                })
            };

            var rule = new MustUseMaxDiceRule();
            var roll = new DiceRoll(new[] { 3, 5 });

            // Act
            var result = rule.Apply(seqences, roll).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Moves.Should().HaveCount(2);
        }
    }
}
