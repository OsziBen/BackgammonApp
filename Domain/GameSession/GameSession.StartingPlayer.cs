using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;

namespace Domain.GameSession
{
    public partial class GameSession
    {
        public StartingPlayerResult DetermineStartingPlayer(
            IReadOnlyList<GamePlayer.GamePlayer> players,
            int roll1,
            int roll2,
            DateTimeOffset now)
        {
            if (IsFinished)
            {
                throw new BusinessRuleException(
                    FunctionCode.BusinessRuleViolation,
                    "Game session already finished");
            }

            if (CurrentPhase != GamePhase.DeterminingStartingPlayer)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidGamePhase,
                    $"Expected phase {GamePhase.DeterminingStartingPlayer}, but was {CurrentPhase}");
            }

            if (players.Count != 2)
            {
                throw new BusinessRuleException(
                    FunctionCode.BusinessRuleViolation,
                    "Exactly two players are required");
            }

            if (roll1 == roll2)
            {
                throw new BusinessRuleException(
                    FunctionCode.BusinessRuleViolation,
                    "Starting rolls must be distinct");
            }

            var p1 = players[0];
            var p2 = players[1];

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
    }
}
