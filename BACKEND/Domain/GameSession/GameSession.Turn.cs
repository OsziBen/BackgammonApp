using Common.Enums.GameSession;

namespace Domain.GameSession
{
    public partial class GameSession
    {
        public void EndTurn(DateTimeOffset now)
        {
            EnsureCanEndTurn();

            var currentPlayerId = GetOpponentOrThrow(CurrentPlayerId!.Value).Id;

            CurrentPlayerId = currentPlayerId;
            LastDiceRoll = null;
            CurrentPhase = CanUseDoublingCube(currentPlayerId)
                ? GamePhase.TurnStart 
                : GamePhase.RollDice;
            LastUpdatedAt = now;
        }
    }
}
