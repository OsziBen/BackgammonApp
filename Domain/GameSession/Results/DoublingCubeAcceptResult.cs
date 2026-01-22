namespace Domain.GameSession.Results
{
    public record DoublingCubeAcceptResult(
        int NewCubeValue,
        Guid OfferingPlayerId,
        Guid AcceptingPlayerId
    );
}
