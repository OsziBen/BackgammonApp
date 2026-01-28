using Common.Enums.BoardState;

namespace Domain.GamePlayer
{
    public static class GamePlayerFactory
    {
        public static GamePlayer CreateHost(
            Guid sessionId,
            Guid userId,
            DateTimeOffset now)
        {
            return new GamePlayer
            {
                GameSessionId = sessionId,
                UserId = userId,
                IsHost = true,
                Color = PlayerColor.White,
                IsConnected = true,
                LastConnectedAt = now
            };
        }
        public static GamePlayer CreateGuest(
            Guid sessionId,
            Guid userId,
            DateTimeOffset now)
        {
            return new GamePlayer
            {
                GameSessionId = sessionId,
                UserId = userId,
                IsHost = false,
                Color = PlayerColor.Black,
                IsConnected = true,
                LastConnectedAt = now
            };
        }
    }
}
