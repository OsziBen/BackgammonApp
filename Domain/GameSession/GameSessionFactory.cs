using Common.Enums.GameSession;

namespace Domain.GameSession
{
    public static class GameSessionFactory
    {
        public static GameSession Create(
            Guid hostPlayerId,
            string sessionCode,
            GameSessionSettings  settings)
        {
            var now = DateTimeOffset.UtcNow;

            return new GameSession
            {
                Id = Guid.NewGuid(),
                MatchId = null,
                CurrentGameId = null,

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

                IsFinished = false,
                WinnerPlayerId = null
            };
        }
    }
}
