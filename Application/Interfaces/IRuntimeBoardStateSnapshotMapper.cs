using Application.Realtime;
using Domain.GameLogic;
using Domain.GameSession;

namespace Application.Interfaces
{
    public interface IRuntimeBoardStateSnapshotMapper
    {
        RuntimeBoardStateSnapshot Map(GameSession session, BoardState boardState);
    }
}
