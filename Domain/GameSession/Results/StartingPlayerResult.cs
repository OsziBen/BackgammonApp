namespace Domain.GameSession.Results
{
    public record StartingPlayerResult(
        Guid StarttingPlayerId,
        IReadOnlyCollection<(Guid PlayerId, int Roll)> Rolls
    );
}
