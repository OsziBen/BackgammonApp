namespace Domain.GameSession.Results
{
    public record StartingPlayerResult(
        IReadOnlyCollection<(Guid PlayerId, int Roll)> Rolls,
        Guid StartingPlayerId
    );
}
