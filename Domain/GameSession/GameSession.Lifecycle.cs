using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GamePlayer;

namespace Domain.GameSession
{
    public partial class GameSession
    {
        public void Start()
        {
            if (IsFinished)
            {
                throw new BusinessRuleException(
                    "Cannot start a finished game session.");
            }

            if (CurrentPhase != GamePhase.WaitingForPlayers)
            {
                throw new BusinessRuleException(
                    $"Cannot start session in phase {CurrentPhase}.");
            }

            if (Players.Count != 2)
            {
                throw new BusinessRuleException(
                    "Game session must have exactly 2 players to start.");
            }

            CurrentPhase = GamePhase.DeterminingStartingPlayer;
            StartedAt ??= DateTimeOffset.UtcNow;
            LastUpdatedAt = DateTimeOffset.UtcNow;
        }

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

        public JoinResult JoinPlayer(Guid userId)
        {
            if (IsFinished)
            {
                throw new BusinessRuleException("Cannot join a finished session."); // TODO: custom exception
            }

            var existingPlayer = Players
                .FirstOrDefault(p => p.UserId == userId);

            if (existingPlayer == null &&
                CurrentPhase != GamePhase.WaitingForPlayers)
            {
                throw new BusinessRuleException(
                    $"Cannot join session in phase {CurrentPhase}.");
            }

            if (existingPlayer != null)
            {
                existingPlayer.IsConnected = true;
                existingPlayer.LastConnectedAt = DateTimeOffset.UtcNow;

                return JoinResult.Rejoined(existingPlayer);
            }

            if (Players.Count >= 2)
            {
                throw new BusinessRuleException("Session is full");
            }

            var newPlayer = Players.Count == 0
                ? GamePlayerFactory.CreateHost(Id, userId)
                : GamePlayerFactory.CreateGuest(Id, userId);

            Players.Add(newPlayer);

            return JoinResult.Joined(newPlayer);
        }


        public bool CanStartGame()
            => Players.Count == 2 &&
                CurrentPhase == GamePhase.WaitingForPlayers;
    }
}
