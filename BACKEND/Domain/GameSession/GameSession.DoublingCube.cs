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

            CurrentPhase = GamePhase.CubeOffered;
            CurrentPlayerId = GetOpponentOrThrow(playerId).Id;
            //LastUpdatedAt = now;

            return new DoublingCubeOfferResult(
                DoublingCubeValue!.Value * 2,
                playerId,
                GetOpponentOrThrow(playerId).Id
            );
        }

        public DoublingCubeAcceptResult AcceptDoublingCube(
            Guid playerId,
            DateTimeOffset now)
        {
            EnsureCanAcceptDoublingCube(playerId);

            var offeringPlayerId = GetOpponentOrThrow(playerId).Id;

            DoublingCubeValue = (DoublingCubeValue ?? 1) * 2;
            DoublingCubeOwnerPlayerId = playerId;

            CurrentPlayerId = offeringPlayerId;
            CurrentPhase = GamePhase.RollDice;
            //LastUpdatedAt = now;

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

            var resultType = boardState.EvaluateForfeitResult(
                forfeitingPlayer: GetPlayerOrThrow(playerId).Color);

            var outcome = GameResultEvaluator.CreateOutcome(
                resultType,
                DoublingCubeValue);

            Finish(GameFinishReason.CubeDeclined, GetOpponentOrThrow(playerId).Id, now);

            return outcome;
        }
    }
}
