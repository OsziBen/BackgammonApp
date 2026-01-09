using Common.Enums.GameSession;

namespace Domain.GameSession
{
    public partial class GameSession
    {
        public void EndTurn()
        {
            if (Players.Count != 2)
            {
                throw new InvalidOperationException("Cannot end turn without 2 players");   // TODO: custom exception
            }

            var currentPlayer = Players.Single(p => p.Id == CurrentPlayerId);

            var nextPlayer = Players.Single(p => p.Id != currentPlayer.Id);

            CurrentPlayerId = nextPlayer.Id;

            LastDiceRoll = null;

            CurrentPhase = GamePhase.RollDice;

            LastUpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
