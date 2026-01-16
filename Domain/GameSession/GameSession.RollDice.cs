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
            EnsureNotFinished();
            EnsurePhase(GamePhase.RollDice);
            EnsureNoActiveDice();
            EnsureCurrentPlayerIsSet();
            EnsureCurrentPlayer(playerId);

            var diceRoll = diceRoller.Roll();

            LastDiceRoll = diceRoll.Values.ToArray();
            CurrentPhase = GamePhase.MoveCheckers;
            LastUpdatedAt = now;

            return diceRoll;
        }
    }
}
