using Application.GameSessions.Requests;
using Domain.GameLogic;

namespace Application.GameSessions.Commands.MoveCheckers
{
    public static class MoveMapper
    {
        public static Move ToDomain(this MoveDto dto)
        {
            return new Move(
                dto.From,
                dto.To,
                dto.Die);
        }
    }
}
