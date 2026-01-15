using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GameSession.Results;

namespace Domain.GameSession
{
    public partial class GameSession
    {
        public StartingPlayerResult DetermineStartingPlayer(
            int roll1,
            int roll2,
            DateTimeOffset now)
        {
            EnsureCanDetermineStartingPlayer();

            if (roll1 == roll2)
            {
                throw new BusinessRuleException(
                    FunctionCode.BusinessRuleViolation,
                    "Starting rolls must be distinct");
            }

            var p1 = Players.ElementAt(0);
            var p2 = Players.ElementAt(1);

            p1.StartingRoll = roll1;
            p2.StartingRoll = roll2;

            var startingPlayer = roll1 > roll2 ? p1 : p2;

            CurrentPlayerId = startingPlayer.Id;
            CurrentPhase = GamePhase.MoveCheckers;
            LastUpdatedAt = now;

            return new StartingPlayerResult(
                startingPlayer.Id,
                new[]
                {
                    (p1.Id, roll1),
                    (p2.Id, roll2)
                });
        }

        public void EndTurn(DateTimeOffset now)
        {
            EnsureCanEndTurn();

            var currentPlayer = Players.Single(p => p.Id == CurrentPlayerId);
            var nextPlayer = Players.Single(p => p.Id != currentPlayer.Id);

            CurrentPlayerId = nextPlayer.Id;
            LastDiceRoll = null;
            CurrentPhase = GamePhase.RollDice;
            LastUpdatedAt = now;
        }
    }
}
