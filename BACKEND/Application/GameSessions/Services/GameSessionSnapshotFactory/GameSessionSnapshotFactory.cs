using Application.GameSessions.Responses;
using Domain.GameSession;

namespace Application.GameSessions.Services.GameSessionSnapshotFactory
{
    public class GameSessionSnapshotFactory : IGameSessionSnapshotFactory
    {
        public GameSessionSnapshotResponse Create(GameSession session, Guid? localPlayerId = null, bool? isRejoin = false)
        {
            return new GameSessionSnapshotResponse
            {
                SessionId = session.Id,
                SessionCode = session.SessionCode,
                CreatedByUserId = session.CreatedByUserId,
                CurrentPhase = session.CurrentPhase,
                CurrentPlayerId = session.CurrentPlayerId,
                BoardStateJson = session.CurrentBoardStateJson,
                LastDiceRoll = session.LastDiceRoll,
                DoublingCubeValue = session.DoublingCubeValue,
                DoublingCubeOwnerPlayerId = session.DoublingCubeOwnerPlayerId,
                CrawfordRuleApplies = session.CrawfordRuleApplies,
                IsFinished = session.IsFinished,
                FinishReason = session.FinishReason,
                WinnerPlayerId = session.WinnerPlayerId,
                ResultType = session.FinalOutcome?.ResultType,
                AwardedPoints = session.FinalOutcome?.Points,
                LocalPlayerId = localPlayerId,
                IsRejoin = isRejoin ?? false,
                Players = session.Players.Select(p => new PlayerSnapshot
                {
                    PlayerId = p.Id,
                    UserId = p.UserId,
                    IsHost = p.IsHost,
                    Color = p.Color,
                    IsConnected = p.IsConnected,
                    StartingRoll = p.StartingRoll
                }).ToList()
            };
        }
    }
}
