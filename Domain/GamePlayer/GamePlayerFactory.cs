using Common.Enums.BoardState;

namespace Domain.GamePlayer
{
    public static class GamePlayerFactory
    {
        public static GamePlayer CreateHost(
            Guid sessionId,
            Guid userId)
        {
            return new GamePlayer
            {
                GameSessionId = sessionId,
                UserId = userId,
                IsHost = true,
                Color = PlayerColor.White,
                IsConnected = true,
                LastConnectedAt = DateTimeOffset.UtcNow
            };
        }
        public static GamePlayer CreateGuest(
            Guid sessionId,
            Guid userId)
        {
            return new GamePlayer
            {
                GameSessionId = sessionId,
                UserId = userId,
                IsHost = false,
                Color = PlayerColor.Black,
                IsConnected = true,
                LastConnectedAt = DateTimeOffset.UtcNow
            };
        }
    }
}
