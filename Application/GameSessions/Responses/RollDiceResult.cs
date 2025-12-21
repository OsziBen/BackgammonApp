namespace Application.GameSessions.Responses
{
    public record RollDiceResult(
        int Die1,
        int Die2,
        int MovesCount
    );
}
