namespace Application.RulesTemplate.Responses
{
    public class RulesTemplateResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string AuthorName { get; set; }

        public int TargetScore { get; set; }
        public bool UseClock { get; set; }
        public int? MatchTimePerPlayerInSeconds { get; set; }
        public int? StartOfTurnDelayPerPlayerInSeconds { get; set; }
        public bool CrawfordRuleEnabled { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
