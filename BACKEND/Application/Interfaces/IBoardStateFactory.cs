using Common.Enums.BoardState;
using Domain.GameLogic;
using Domain.GameSession;

namespace Application.Interfaces
{
    public interface IBoardStateFactory
    {
        BoardState Create(GameSession session);
        BoardState CreateInitial(GameSession session);
    }
}
