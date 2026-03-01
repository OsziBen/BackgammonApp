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

        public void Finish(GameFinishReason reason, Guid winnerPlayerId, DateTimeOffset now)
        {
            IsFinished = true;
            FinishedAt = now;
            WinnerPlayerId = winnerPlayerId;
            CurrentPhase = GamePhase.GameFinished;
            FinishReason = reason;
        }

        public void Abandon(DateTimeOffset now)
        {
            IsFinished = true;
            FinishedAt = now;
            WinnerPlayerId = null;
            CurrentPhase = GamePhase.GameAbandoned;
            FinishReason = GameFinishReason.Abandoned;
        }

        public void Forfeit(
            Guid forfeitingPlayerId,
            BoardState boardState,
            DateTimeOffset now)
        {
            EnsureCanForfeit(forfeitingPlayerId);

            var forfeitingPlayer = GetPlayerOrThrow(forfeitingPlayerId);

            var winner = GetOpponentOrThrow(forfeitingPlayerId);

            var resultType = boardState.EvaluateForfeitResult(
                forfeitingPlayer!.Color);

            FinalOutcome = GameResultEvaluator.CreateOutcome(
                resultType,
                DoublingCubeValue);

            Finish(GameFinishReason.Forfeit, winner.Id, now);
        }

        public JoinResult JoinPlayer(Guid userId, DateTimeOffset now)
        {
            EnsureNotFinished();

            var existingPlayer = Players
                .FirstOrDefault(p => p.UserId == userId);

            if (existingPlayer != null)
            {
                existingPlayer.Connect(now);

                return JoinResult.Rejoined(Id, existingPlayer);
            }

            EnsureCanJoin();

            var newPlayer = Players.Count == 0
                ? GamePlayerFactory.CreateHost(Id, userId)
                : GamePlayerFactory.CreateGuest(Id, userId);

            Players.Add(newPlayer);
            newPlayer.Connect(now);

            return JoinResult.Joined(Id, newPlayer);
        }

        public void DetermineStartingPlayer(
            IStartingPlayerRoller roller,
            DateTimeOffset now)
        {
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
            LastDiceRoll = [roll.Player1Roll, roll.Player2Roll];
            LastUpdatedAt = now;
        }

        public bool CanTimeoutPlayer()
        {
            if (IsFinished || !IsInActivePlayPhase() || Settings.ClockEnabled)
            {
                return false;
            }

            return true;
        }

        public bool CanStartGame()
            => Players.Count == 2 &&
                CurrentPhase == GamePhase.WaitingForPlayers;

        public bool CanUseDoublingCube(Guid playerId)
        {
            if (CrawfordRuleApplies || DoublingCubeOwnerPlayerId != playerId)
            {
                return false;
            }

            return true;
        }

        private bool IsInActivePlayPhase()
        {
            return CurrentPhase == GamePhase.TurnStart
                || CurrentPhase == GamePhase.RollDice
                || CurrentPhase == GamePhase.MoveCheckers
                || CurrentPhase == GamePhase.CubeOffered;
        }
    }
}
