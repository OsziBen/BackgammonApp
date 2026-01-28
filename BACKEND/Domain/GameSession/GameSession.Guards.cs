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
                    FunctionCode.InsufficientPlayerNumber,
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
                    "Only the current player can perform this action.");
            }
        }

        private void EnsureCurrentPlayerIsSet()
        {
            if (CurrentPlayerId == null)
                throw new BusinessRuleException(
                    FunctionCode.InvalidGameState,
                    "Current player is not set");
        }

        private void EnsureNoActiveDice()
        {
            if (LastDiceRoll != null)
            {
                throw new BusinessRuleException(
                    FunctionCode.DiceAlreadyRolled,
                    "Dice already rolled for this turn.");
            }
        }

        private void EnsureCanJoin()
        {
            EnsurePhase(GamePhase.WaitingForPlayers);

            if (Players.Count >= 2)
            {
                throw new BusinessRuleException(
                    FunctionCode.SessionFull,
                    "Session is full.");
            }
        }

        private void EnsureDoublingCubeEnabled()
        {
            if (!Settings.DoublingCubeEnabled)
            {
                throw new BusinessRuleException(
                    FunctionCode.DoublingCubeDisabled,
                    "Doubling cube is disabled for this session.");
            }
        }

        private void EnsurePlayerMayUseDoublingCube(Guid playerId)
        {
            if (DoublingCubeOwnerPlayerId == null)
                return;

            if (DoublingCubeOwnerPlayerId == playerId)
                return;

            throw new BusinessRuleException(
                FunctionCode.PlayerDoesNotPossessDoublingCube,
                "Player does not have the right to offer the doubling cube.");
        }


        private void EnsureCanOfferDoublingCube(Guid playerId)
        {
            EnsureNotFinished();
            EnsurePhase(GamePhase.TurnStart);
            EnsureCurrentPlayer(playerId);
            EnsureDoublingCubeEnabled();
            EnsurePlayerMayUseDoublingCube(playerId);
        }

        private void EnsureCanAcceptDoublingCube(Guid playerId)
        {
            EnsureNotFinished();
            EnsurePhase(GamePhase.CubeOffered);
            EnsurePlayerIsInSession(playerId);

            if (playerId == CurrentPlayerId)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidPlayer,
                    "Offering player cannot accept their own cube.");
            }
        }
        private void EnsureCanDeclineDoublingCube(Guid playerId)
        {
            EnsureNotFinished();
            EnsurePhase(GamePhase.CubeOffered);
            EnsurePlayerIsInSession(playerId);

            if (playerId == CurrentPlayerId)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidPlayer,
                    "Offering player cannot decline their own cube.");
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
