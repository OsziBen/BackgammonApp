namespace Domain.GameSession
{
    public record StartingPlayerResult(
        Guid StarttingPlayerId,
        IReadOnlyCollection<(Guid PlayerId, int Roll)> Rolls
    );
}
