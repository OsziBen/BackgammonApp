using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GameSession.Results;

namespace Domain.GameSession
{
    public partial class GameSession
    {
        public void EndTurn(DateTimeOffset now)
        {
            EnsureCanEndTurn();

            var currentPlayer = Players.First(p => p.Id == CurrentPlayerId);
            var nextPlayer = Players.First(p => p.Id != currentPlayer.Id);

            CurrentPlayerId = nextPlayer.Id;
            LastDiceRoll = null;
            CurrentPhase = GamePhase.RollDice;
            LastUpdatedAt = now;
        }
    }
}
