using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using Common.Models;
using Domain.GameLogic;
using Domain.GameSession.Results;

namespace Domain.GameSession
{
    public partial class GameSession : BaseEntity
    {
        public int Version { get; private set; } = 0;
        public Guid? MatchId { get; set; }
        public Guid? CurrentGameId { get; set; }
        public Guid CreatedByUserId { get; set; }

        public string SessionCode { get; set; } = null!;
        public GameSessionSettings Settings { get; set; } = null!;

        public GamePhase CurrentPhase { get; set; }
        public Guid? CurrentPlayerId { get; set; }
        public int[]? LastDiceRoll { get; set; }
        public string? CurrentBoardStateJson { get; set; }

        public int? DoublingCubeValue { get; set; }
        public Guid? DoublingCubeOwnerPlayerId { get; set; }
        public bool CrawfordRuleApplies { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? FinishedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public bool IsFinished { get; set; }
        public GameFinishReason? FinishReason { get; set; }
        public Guid? WinnerPlayerId { get; set; }
        public GameOutcome? FinalOutcome { get; set; }

        public User.User Creator { get; set; } = null!;
        public Match.Match? Match { get; set; }
        public Game.Game? CurrentGame { get; set; }
        public GamePlayer.GamePlayer? WinnerPlayer { get; set; }
        public ICollection<GamePlayer.GamePlayer> Players { get; set; } = [];

        public void IncrementVersion()
            => Version++;

        public void UpdateBoardState(string boardStateSnapshot)
        {
            CurrentBoardStateJson = boardStateSnapshot;
        }

        public void MarkUpdated(DateTimeOffset now)
            => LastUpdatedAt = now;

        public void MarkDeleted(DateTimeOffset now)
        {
            IsDeleted = true;
            DeletedAt = now;
        }

        public GamePlayer.GamePlayer GetPlayerOrThrow(Guid playerId)
        {
            EnsureExactlyTwoPlayers();

            var player = Players.FirstOrDefault(p => p.Id == playerId);

            return player ?? throw new InvalidOperationException(
                    $"Player {playerId} not found in session.");
        }

        public GamePlayer.GamePlayer GetOpponentOrThrow(Guid playerId)
        {
            EnsureExactlyTwoPlayers();

            var playersArray = Players.ToArray();

            if (playersArray[0].Id == playerId)
            {
                return playersArray[0];
            }

            if (playersArray[1].Id == playerId)
            {
                return playersArray[1];
            }

            throw new InvalidOperationException(
                $"Player {playerId} not found in session.");
        }

        public DiceRoll GetCurrentDiceRoll()
        {
            EnsurePhase(GamePhase.MoveCheckers);

            if (LastDiceRoll == null)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidGameState,
                    "Dice has not been rolled.");
            }

            return new DiceRoll(LastDiceRoll);
        }

        public void EvaluateCrawfordRule()
        {
            if (!Settings.CrawfordRuleEnabled)
            {
                CrawfordRuleApplies = false;
                return;
            }

            // TODO: real Crawford logic (not MVP)
            CrawfordRuleApplies = false;
        }

    }
}
