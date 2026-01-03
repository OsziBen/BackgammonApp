using Application.Interfaces;
using Application.Realtime;
using Common.Enums;
using Common.Exceptions;
using Domain.GameLogic;
using Domain.GameSession;
using System.Text.Json;

namespace Infrastructure.Realtime.Factories
{
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

            var runtimeState = JsonSerializer.Deserialize<RuntimeBoardStateSnapshot>(
                session.CurrentBoardStateJson) ??
                throw new NotFoundException(
                    FunctionCode.ResourceNotFound,
                    "Failed to parse board state JSON");

            var points = runtimeState.Points
                .ToDictionary(
                    p => p.Key,
                    p => new CheckerPosition(p.Value.Owner, p.Value.Count)
                );

            return new BoardState(
                points,
                runtimeState.BarWhite,
                runtimeState.BarBlack,
                runtimeState.OffWhite,
                runtimeState.OffBlack,
                runtimeState.CurrentPlayer
                );
        }
    }
}
