using Application.Interfaces;
using Application.Realtime;
using Common.Enums;
using Common.Enums.BoardState;
using Common.Exceptions;
using Domain.GameLogic;
using Domain.GameLogic.Constants;
using Domain.GameSession;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Realtime.Factories
{
    public static class JsonOptionsCache
    {
        public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            PropertyNameCaseInsensitive = true
        };
    }

    public class BoardStateFactory : IBoardStateFactory
    {
        public BoardState Create(GameSession session)
        {
            if (session.CurrentBoardStateJson == null)
            {
                throw new NotFoundException(
                    FunctionCode.ResourceNotFound,
                    "Board state is missing from session");
            }

            Console.WriteLine("RAW JSON:");
            Console.WriteLine(session.CurrentBoardStateJson);

            var runtimeState = JsonSerializer.Deserialize<RuntimeBoardStateSnapshot>(
                session.CurrentBoardStateJson, JsonOptionsCache.Options) ??
                throw new NotFoundException(
                    FunctionCode.ResourceNotFound,
                    "Failed to parse board state JSON");

            if (runtimeState.Points == null || runtimeState.Points.Count == 0)
            {
                throw new Exception("CRITICAL: Points deserialized as empty!");

            }
            var points = runtimeState.Points
                .ToDictionary(
                    p => int.Parse(p.Key),
                    p => new CheckerPosition(p.Value.Owner, p.Value.Count)
                );

            Console.WriteLine($"Points parsed: {points.Count}");

            return new BoardState(
                points,
                runtimeState.BarWhite,
                runtimeState.BarBlack,
                runtimeState.OffWhite,
                runtimeState.OffBlack,
                runtimeState.CurrentPlayer
                );
        }

        public BoardState CreateInitial(GameSession session)
        {
            var points = new Dictionary<int, CheckerPosition>();

            // Empty board
            for (int i = 1; i <= BoardConstants.BoardPoints; i++)
            {
                points[i] = new CheckerPosition(null, 0);
            }

            // White
            points[1] = new CheckerPosition(PlayerColor.White, 2);
            points[12] = new CheckerPosition(PlayerColor.White, 5);
            points[17] = new CheckerPosition(PlayerColor.White, 3);
            points[19] = new CheckerPosition(PlayerColor.White, 5);

            // Black
            points[6] = new CheckerPosition(PlayerColor.Black, 5);
            points[8] = new CheckerPosition(PlayerColor.Black, 3);
            points[13] = new CheckerPosition(PlayerColor.Black, 5);
            points[24] = new CheckerPosition(PlayerColor.Black, 2);

            return new BoardState(
                points,
                barWhite: 0,
                barBlack: 0,
                offWhite: 0,
                offBlack: 0,
                currentPlayer: session.GetPlayerOrThrow(session.CurrentPlayerId!.Value).Color
            );
        }
    }
}
