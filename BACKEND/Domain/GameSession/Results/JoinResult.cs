namespace Domain.GameSession.Results
{
    public class JoinResult
    {
        public Guid SessionId { get; set; }
        public GamePlayer.GamePlayer Player { get; }
        public bool IsRejoin { get; }

        private JoinResult(
            Guid sessionId,
            GamePlayer.GamePlayer player,
            bool isRejoin)
        {
            SessionId = sessionId;
            Player = player;
            IsRejoin = isRejoin;
        }

        public static JoinResult Joined(Guid sessionId, GamePlayer.GamePlayer player)
            => new(sessionId, player, isRejoin: false);

        public static JoinResult Rejoined(Guid sessionId, GamePlayer.GamePlayer player)
            => new(sessionId, player, isRejoin: true);
    }
}
