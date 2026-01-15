using Common.Enums.Game;
using Common.Enums.GameSession;
using Common.Exceptions;
using Domain.GameLogic;
using Domain.GamePlayer;
using Domain.GameSession.Results;

namespace Domain.GameSession
{
    public partial class GameSession
    {
        public void Start(DateTimeOffset now)
        {
            EnsureCanStartGame();

            CurrentPhase = GamePhase.DeterminingStartingPlayer;
            StartedAt ??= now;
            LastUpdatedAt = now;
        }

        public void Finish(Guid winnerPlayerId, DateTimeOffset now)
        {
            IsFinished = true;
            FinishedAt = now;
            WinnerPlayerId = winnerPlayerId;
            CurrentPhase = GamePhase.GameFinished;
        }

        public GameResultType Forfeit(
            Guid forfeitingPlayerId,
            BoardState boardState,
            DateTimeOffset now)
        {
            EnsureNotFinished();
            EnsureExactlyTwoPlayers();
            EnsurePlayerIsInSession(forfeitingPlayerId);

            var winner = Players.Single(p => p.Id != forfeitingPlayerId);

            var resultType = GameResultEvaluator.Evaluate(
                boardState,
                winner.Color,
                DoublingCubeValue);

            Finish(winner.Id, now);

            return resultType;
        }

        public JoinResult JoinPlayer(Guid userId, DateTimeOffset now)
        {
            EnsureNotFinished();

            var existingPlayer = Players
                .FirstOrDefault(p => p.UserId == userId);

            if (existingPlayer == null &&   // TODO: refactor
                CurrentPhase != GamePhase.WaitingForPlayers)
            {
                throw new BusinessRuleException(
                    $"Cannot join session in phase {CurrentPhase}.");
            }

            if (existingPlayer != null)
            {
                existingPlayer.IsConnected = true;
                existingPlayer.LastConnectedAt = now;

                return JoinResult.Rejoined(existingPlayer);
            }

            if (Players.Count >= 2)
            {
                throw new BusinessRuleException("Session is full.");
            }

            var newPlayer = Players.Count == 0
                ? GamePlayerFactory.CreateHost(Id, userId, now)
                : GamePlayerFactory.CreateGuest(Id, userId, now);

            Players.Add(newPlayer);

            return JoinResult.Joined(newPlayer);
        }

        public bool CanStartGame()
            => Players.Count == 2 &&
                CurrentPhase == GamePhase.WaitingForPlayers;
    }
}
