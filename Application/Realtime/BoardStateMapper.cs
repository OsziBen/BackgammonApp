using Domain.GameLogic;
using System.Text.Json;

namespace Application.Realtime
{
    public static class BoardStateMapper
    {
        public static string ToJson(BoardState state)
        {
            var snapshot = RuntimeBoardStateSnapshot.FromDomain(state);

            return JsonSerializer.Serialize(snapshot);
        }
    }
}
