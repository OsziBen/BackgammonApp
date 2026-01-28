namespace Common.Constants
{
    public static class GameSessionConstants
    {
        public const int MaxPlayers = 2;
        public static readonly TimeSpan DisconnectTimeout
            = TimeSpan.FromSeconds(60);
    }
}
