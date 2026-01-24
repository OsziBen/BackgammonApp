using Common.Enums.GameSession;
using Domain.GameLogic;
using Domain.GameSession.Results;
using Domain.GameSession.Services;

namespace Domain.GameSession
{
    public partial class GameSession
    {
        public DoublingCubeOfferResult OfferDoublingCube(
            Guid playerId,
            DateTimeOffset now)
        {
            EnsureCanOfferDoublingCube(playerId);

            DoublingCubeValue ??= 1;
            var offeredValue = DoublingCubeValue!.Value * 2;

            CurrentPhase = GamePhase.CubeOffered;
            LastUpdatedAt = now;

            return new DoublingCubeOfferResult(
                offeredValue,
                playerId,
                GetOpponentId(playerId)
            );
        }

        public DoublingCubeAcceptResult AcceptDoublingCube(
                    Guid playerId,
                    DateTimeOffset now)
        {
            EnsureCanAcceptDoublingCube(playerId);

            var offeringPlayerId = GetOpponentId(playerId);

            DoublingCubeValue = (DoublingCubeValue ?? 1) * 2;
            DoublingCubeOwnerPlayerId = playerId;

            CurrentPlayerId = offeringPlayerId;
            CurrentPhase = GamePhase.RollDice;
            LastUpdatedAt = now;

            return new DoublingCubeAcceptResult(
                DoublingCubeValue.Value,
                offeringPlayerId,
                playerId);
        }

        public GameOutcome DeclineDoublingCube(
            Guid playerId,
            BoardState boardState,
            DateTimeOffset now)
        {
            EnsureCanDeclineDoublingCube(playerId);

            var winner = GetOpponent(playerId);

            var resultType = boardState.EvaluateForfeitResult(
                forfeitingPlayer: GetPlayerColor(playerId));

            var outcome = GameResultEvaluator.CreateOutcome(
                resultType,
                DoublingCubeValue);

            Finish(winner.Id, now);

            return outcome;
        }
    }
}
