namespace Domain.GameSession
{
    public partial class GameSession
    {
        public void Finish(Guid winnerPlayerId)
        {
            IsFinished = true;
            FinishedAt = DateTimeOffset.UtcNow;
            WinnerPlayerId = winnerPlayerId;
        }

        public void Forfeit(Guid playerId)
        {
            if (IsFinished)
            {
                throw new InvalidOperationException("Game already finished");
            }

            var winner = Players.Single(p => p.Id != playerId);
            Finish(winner.Id);
        }
    }
}
