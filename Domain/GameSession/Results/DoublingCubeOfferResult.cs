namespace Domain.GameSession.Results
{
    public record DoublingCubeOfferResult(
        int OfferedCubeValue,
        Guid OfferingPlayerId,
        Guid ReceivingPlayerId
    );

}
