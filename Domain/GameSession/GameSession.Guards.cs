using Common.Enums;
using Common.Enums.GameSession;
using Common.Exceptions;

namespace Domain.GameSession
{
    public partial class GameSession
    {
        private void EnsureExactlyTwoPlayers()
        {
            if (Players.Count != 2)
            {
                throw new BusinessRuleException(
                    FunctionCode.BusinessRuleViolation,
                    "Exactly two players are required.");
            }
        }
        private void EnsurePhase(GamePhase expected)
        {
            if (CurrentPhase != expected)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidGamePhase,
                    $"Expected phase {expected}, but was {CurrentPhase}.");
            }
        }

        private void EnsureNotFinished()
        {
            if (IsFinished)
            {
                throw new BusinessRuleException(
                    FunctionCode.GameAlreadyFinished,
                    "Game session already finished.");
            }
        }

        private void EnsureDiceRolled()
        {
            if (LastDiceRoll == null || LastDiceRoll.Length == 0)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidGameState,
                    "Dice not rolled.");
            }
        }

        private void EnsurePlayerIsInSession(Guid playerId)
        {
            if (Players.All(p => p.Id != playerId))
            {
                throw new BusinessRuleException(
                    FunctionCode.PlayerNotInSession,
                    "Player is not part of this session.");
            }
        }


        private void EnsureCurrentPlayer(Guid playerId)
        {
            if (CurrentPlayerId != playerId)
            {
                throw new BusinessRuleException(
                    FunctionCode.NotYourTurn,
                    "Only the current player can move.");
            }
        }


        private void EnsureCanStartGame()
        {
            EnsureNotFinished();
            EnsurePhase(GamePhase.WaitingForPlayers);
            EnsureExactlyTwoPlayers();
        }

        private void EnsureCanEndTurn()
        {
            EnsureNotFinished();
            EnsurePhase(GamePhase.MoveCheckers);
            EnsureExactlyTwoPlayers();
        }

        private void EnsureCanDetermineStartingPlayer()
        {
            EnsureNotFinished();
            EnsurePhase(GamePhase.DeterminingStartingPlayer);
            EnsureExactlyTwoPlayers();
        }

        private void EnsureCanMoveCheckers()
        {
            EnsureNotFinished();
            EnsurePhase(GamePhase.MoveCheckers);
            EnsureDiceRolled();
        }
    }
}
