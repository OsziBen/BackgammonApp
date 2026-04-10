using Domain.GameLogic;
using Domain.GameSession;
using System.Text.Json;

namespace Application.Realtime
{
    public static class BoardStateMapper
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static string ToJson(
            GameSession session,
            BoardState state)
        {
            var snapshot = RuntimeBoardStateSnapshotMapper.Map(session, state);

            return JsonSerializer.Serialize(snapshot, _options);
        }
    }
}