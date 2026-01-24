using Common.Enums.GameSession;
using Domain.GameLogic;
using Domain.GamePlayer;
using Domain.GameSession.Results;
using Domain.GameSession.Services;

namespace Domain.GameSession
{
    public partial class GameSession
    {
        public void Start(DateTimeOffset now)
        {
            EnsureCanStartGame();

            ApplyMatchRules();

            CurrentPhase = GamePhase.DeterminingStartingPlayer;
            StartedAt ??= now;
            LastUpdatedAt = now;
        }

        private void ApplyMatchRules()
        {
            EvaluateCrawfordRule();
        }

        public void Finish(Guid winnerPlayerId, DateTimeOffset now)
        {
            IsFinished = true;
            FinishedAt = now;
            WinnerPlayerId = winnerPlayerId;
            CurrentPhase = GamePhase.GameFinished;
        }

        public GameOutcome Forfeit(
            Guid forfeitingPlayerId,
            BoardState boardState,
            DateTimeOffset now)
        {
            EnsureNotFinished();
            EnsureExactlyTwoPlayers();
            EnsurePlayerIsInSession(forfeitingPlayerId);

            var forfeitingPlayer = Players
                .First(p => p.Id == forfeitingPlayerId);

            var winner = GetOpponent(forfeitingPlayerId);

            var resultType = boardState.EvaluateForfeitResult(
                forfeitingPlayer.Color);

            var outcome = GameResultEvaluator.CreateOutcome(
                resultType,
                DoublingCubeValue);

            Finish(winner.Id, now);

            return outcome;
        }

        public JoinResult JoinPlayer(Guid userId, DateTimeOffset now)
        {
            EnsureNotFinished();

            var existingPlayer = Players
                .FirstOrDefault(p => p.UserId == userId);

            if (existingPlayer != null)
            {
                existingPlayer.IsConnected = true;
                existingPlayer.LastConnectedAt = now;

                return JoinResult.Rejoined(existingPlayer);
            }

            EnsureCanJoin();

            var newPlayer = Players.Count == 0
                ? GamePlayerFactory.CreateHost(Id, userId, now)
                : GamePlayerFactory.CreateGuest(Id, userId, now);

            Players.Add(newPlayer);

            return JoinResult.Joined(newPlayer);
        }

        public StartingPlayerResult DetermineStartingPlayer(
            IStartingPlayerRoller roller,
            DateTimeOffset now)
        {
            EnsureNotFinished();
            EnsureCanDetermineStartingPlayer();

            var roll = roller.Roll();

            var player1 = Players.ElementAt(0);
            var player2 = Players.ElementAt(1);

            player1.StartingRoll = roll.Player1Roll;
            player2.StartingRoll = roll.Player2Roll;

            var startingPlayer =
                roll.Player1Roll > roll.Player2Roll
                    ? player1
                    : player2;

            CurrentPlayerId = startingPlayer.Id;
            CurrentPhase = GamePhase.MoveCheckers;
            LastUpdatedAt = now;

            return new StartingPlayerResult(
                [
                    (player1.Id, roll.Player1Roll),
                    (player2.Id, roll.Player2Roll)
                ],
                startingPlayer.Id
            );
        }

        public bool CanStartGame()
            => Players.Count == 2 &&
                CurrentPhase == GamePhase.WaitingForPlayers;
    }
}
