using Common.Enums.GameSession;
using Domain.GameLogic;

namespace Domain.GameSession
{
    public partial class GameSession
    {
        public DiceRoll RollDice(
            Guid playerId,
            IDiceRoller diceRoller,
            DateTimeOffset now)
        {
            EnsureCanRollDice(playerId);

            var diceRoll = diceRoller.Roll();

            LastDiceRoll = diceRoll.Values.ToArray();
            CurrentPhase = GamePhase.MoveCheckers;
            LastUpdatedAt = now;

            return diceRoll;
        }

        private bool CanRollDice(Guid playerId)
            => CurrentPhase == GamePhase.RollDice
                || (CurrentPhase == GamePhase.TurnStart && CanUseDoublingCube(playerId));

    }
}
