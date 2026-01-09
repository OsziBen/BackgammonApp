namespace Domain.GameSession
{
    public class GameSessionSettings
    {
        public int TargerPoints { get; init; } = 1;

        public bool DoublingCubeEnabled { get; init; }

        public bool ClockEnabled { get; init; }
        public int? MatchTimePerPlayerInSeconds { get; init; }
        public int? StartOfTurnDelayPerPlayerInSeconds { get; init; }

        public bool CrawfordRuleEnabled { get; init; }
    }
}
