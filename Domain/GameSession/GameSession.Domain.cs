using Common.Enums.GameSession;

namespace Domain.GameSession
{
    public partial class GameSession
    {
        public void UpdateBoardStateJson(string boardStateJson)
        {
            CurrentBoardStateJson = boardStateJson;
        }

        public void EndTurn()
        {
            CurrentPhase = GamePhase.RollDice;
            RemainingMoves = 0;
        }

        public void Finish(Guid winnerPlayerId)
        {
            IsFinished = true;
            FinishedAt = DateTimeOffset.UtcNow;
            WinnerPlayerId = winnerPlayerId;
        }
    }
}
