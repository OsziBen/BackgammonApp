using Common.Enums.Game;

namespace Domain.GameSession.Results
{
    public record GameOutcome(
        GameResultType ResultType,
        int Points
    );
}
