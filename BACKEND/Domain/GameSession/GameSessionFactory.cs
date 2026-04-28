using Common.Enums.GameSession;
using Domain.GamePlayer;

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
            var session = new GameSession
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
                FinishReason = null,
                WinnerPlayerId = null,

                Players = []
            };

            session.Players.Add(GamePlayerFactory.CreateHost(session.Id, userId));

            return session;
        }
    }
}
