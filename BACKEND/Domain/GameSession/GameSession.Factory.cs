using Common.Enums.GameSession;

namespace Domain.GameSession
{
    public static class GameSessionFactory
    {
        public static GameSession Create(
            Guid userId,
            string sessionCode,
            GameSessionSettings settings,
            DateTimeOffset now)
        {
            return new GameSession
            {
                Id = Guid.NewGuid(),
                MatchId = null,
                CurrentGameId = null,
                CreatedByUserId = userId,

                SessionCode = sessionCode,
                Settings = settings,

                CurrentPhase = GamePhase.WaitingForPlayers,
                CurrentPlayerId = null,
                LastDiceRoll = null,
                CurrentBoardStateJson = null,

                DoublingCubeValue = settings.DoublingCubeEnabled ? 1 : null,
                DoublingCubeOwnerPlayerId = null,

                CreatedAt = now,
                LastUpdatedAt = now,
                StartedAt = null,
                FinishedAt = null,

                IsDeleted = false,
                DeletedAt = null,

                IsFinished = false,
                WinnerPlayerId = null
            };
        }
    }
}
