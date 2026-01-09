using Domain.GameLogic;
using Domain.GameSession;

namespace Application.Realtime
{
    public static class RuntimeBoardStateSnapshotFactory
    {
        public static RuntimeBoardStateSnapshot Create(
            GameSession session,
            BoardState boardState)
        {
            return new RuntimeBoardStateSnapshot{
                Points = boardState.Points.ToDictionary(
                    p => p.Key,
                    p => p.Value.Clone()),
                BarWhite = boardState.BarWhite,
                BarBlack = boardState.BarBlack,
                OffWhite = boardState.OffWhite,
                OffBlack = boardState.OffBlack,
                CurrentPlayer = boardState.CurrentPlayer,
                CurrentGamePhase = session.CurrentPhase,
                DiceRoll = session.LastDiceRoll == null
                ? null
                : new DiceRoll(session.LastDiceRoll)
            };
        }
    }
}
