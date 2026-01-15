using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;
using Common.Models;
using Domain.GameLogic;

namespace Domain.GameSession
{
    public partial class GameSession : BaseEntity
    {
        public Guid? MatchId { get; set; }
        public Guid? CurrentGameId { get; set; }
        
        public string SessionCode { get; set; } = null!;
        public GameSessionSettings Settings { get; set; } = null!;

        public GamePhase CurrentPhase { get; set; }
        public Guid? CurrentPlayerId { get; set; }
        public int[]? LastDiceRoll { get; set; }
        public string? CurrentBoardStateJson { get; set; }

        public int? DoublingCubeValue { get; set; }
        public Guid? DoublingCubeOwnerPlayerId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? FinishedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }

        public bool IsFinished { get; set; }
        public Guid? WinnerPlayerId { get; set; }

        public Match.Match? Match { get; set; }
        public Game.Game? CurrentGame { get; set; }
        public GamePlayer.GamePlayer? WinnerPlayer { get; set; }
        public ICollection<GamePlayer.GamePlayer> Players { get; set; } = [];


        public void UpdateBoardState(string boardStateSnapshot)
        {
            CurrentBoardStateJson = boardStateSnapshot;
        }

        private GamePlayer.GamePlayer GetCurrentPlayer()
        {
            if (CurrentPlayerId == null)
            {
                throw new InvalidOperationException(
                    "CurrentPlayerId is not set");
            }

            var player = Players.SingleOrDefault(
                p => p.Id == CurrentPlayerId.Value);

            if (player == null)
            {
                throw new InvalidOperationException(
                    $"Current player {CurrentPlayerId} not found in session");
            }

            return player;
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
    }
}
