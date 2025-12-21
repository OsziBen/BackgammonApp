using Common.Enums.GameSession;

namespace Domain.GameSession
{
    public static class GameSessionFactory
    {
        public static GameSession Create(
            Guid matchId,
            string sessionCode)
        {
            var now = DateTimeOffset.UtcNow;

            return new GameSession
            {
                Id = Guid.NewGuid(),
                MatchId = matchId,
                SessionCode = sessionCode,

                CurrentPhase = GamePhase.WaitingForPlayers,
                CurrentPlayerId = null,
                LastDiceRoll = null,
                RemainingMoves = null,
                CurrentBoardStateJson = null,

                CreatedAt = now,
                StartedAt = null,
                FinishedAt = null,
                LastUpdatedAt = now,

                IsFinished = false,
                WinnerPlayerId = null
            };
        }
    }
}
