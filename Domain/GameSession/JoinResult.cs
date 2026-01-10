namespace Domain.GameSession
{
    public class JoinResult
    {
        public GamePlayer.GamePlayer Player { get; }
        public bool IsRejoin { get; }

        private JoinResult(
            GamePlayer.GamePlayer player,
            bool isRejoin)
        {
            Player = player;
            IsRejoin = isRejoin;
        }

        public static JoinResult Joined(GamePlayer.GamePlayer player)
            => new(player, isRejoin: false);

        public static JoinResult Rejoined(GamePlayer.GamePlayer player)
            => new(player, isRejoin: true);
    }
}
