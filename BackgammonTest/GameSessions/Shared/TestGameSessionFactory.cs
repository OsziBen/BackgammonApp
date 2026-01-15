using Common.Enums.GameSession;
using Domain.GamePlayer;
using Domain.GameSession;

namespace BackgammonTest.GameSessions.Shared
{
    public static class TestGameSessionFactory
    {
        public static GameSession CreateEmptySession(
            GamePhase phase,
            DateTimeOffset? now = null)
        {
            var time = now ?? DateTimeOffset.UtcNow;

            var session = new GameSession
            {
                Id = Guid.NewGuid(),
                SessionCode = "TEST23",
                CurrentPhase = phase,
                CreatedAt = time,
                LastUpdatedAt = time,
                Players = [],
                IsFinished = false
            };

            ApplyPhaseInvariants(session, phase, time);

            return session;
        }

        public static GameSession CreateValidSession(
            GamePhase phase,
            DateTimeOffset? now = null)
        {
            var time = now ?? DateTimeOffset.UtcNow;

            var session = CreateEmptySession(phase, now);

            session.Players.Add(
                GamePlayerFactory.CreateHost(
                    session.Id,
                    Guid.NewGuid(),
                    time)
                );

            session.Players.Add(
                GamePlayerFactory.CreateGuest(
                    session.Id,
                    Guid.NewGuid(),
                    time)
                );

            ApplyPhaseInvariants(session, phase, time);

            return session;
        }

        private static void ApplyPhaseInvariants(
            GameSession session,
            GamePhase phase,
            DateTimeOffset now)
        {
            session.CurrentPhase = phase;
            session.LastUpdatedAt = now;

            switch (phase)
            {
                case GamePhase.WaitingForPlayers:
                    session.StartedAt = null;
                    session.CurrentPlayerId = null;
                    break;

                case GamePhase.DeterminingStartingPlayer:
                    session.StartedAt ??= now;
                    break;

                case GamePhase.RollDice:
                    break;

                case GamePhase.MoveCheckers:
                    session.StartedAt ??= now;
                    break;

                case GamePhase.GameFinished:
                    session.IsFinished = true;
                    session.FinishedAt ??= now;
                    break;

                case GamePhase.GameAbandoned:
                    session.IsFinished = true;
                    session.FinishedAt ??= now;
                    break;

                default:
                    break;
            }
        }
    }
}
