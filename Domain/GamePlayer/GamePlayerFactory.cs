using Common.Enums.BoardState;

namespace Domain.GamePlayer
{
    public static class GamePlayerFactory
    {
        public static GamePlayer Create(
            Guid sessionId,
            Guid userId,
            bool isHost)
        {
            return new GamePlayer
            {
                Id = Guid.NewGuid(),
                GameSessionId = sessionId,
                UserId = userId,
                IsHost = isHost,
                Color = isHost
                    ? PlayerColor.White
                    : PlayerColor.Black,
                IsConnected = true,
                LastConnectedAt = DateTimeOffset.UtcNow,
                StartingRoll = null
            };
        }
    }
}
