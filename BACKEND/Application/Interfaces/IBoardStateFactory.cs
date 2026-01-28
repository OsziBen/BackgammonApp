using Domain.GameLogic;
using Domain.GameSession;

namespace Application.Interfaces
{
    public interface IBoardStateFactory
    {
        BoardState Create(GameSession session);
    }
}
