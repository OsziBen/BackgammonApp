using Application.GameSessions.Realtime;
using Application.GameSessions.Requests;
using Common.Enums.GameSession;
using Domain.GameSession.Results;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Realtime
{
    public class SignalRGameSessionNotifier : IGameSessionNotifier
    {
        private readonly IHubContext<GameSessionHub> _hub;

        public SignalRGameSessionNotifier(IHubContext<GameSessionHub> hub)
        {
            _hub = hub;
        }

        public Task CheckersMoved(
            Guid sessionId,
            Guid playerId,
            IReadOnlyList<MoveDto> moves)
            => _hub.Clients.Group(sessionId.ToString())
                .SendAsync(
                    "CheckerMoved",
                    new
                    {
                        SessionId = sessionId,
                        PlayerId = playerId,
                        Moves = moves
                    });

        public Task DiceRolled(
            Guid sessionId,
            Guid playerId,
            int die1,
            int die2)
            => _hub.Clients
                .Group(sessionId.ToString())
                .SendAsync(
                    "DiceRolled",
                    new
                    {
                        SessionId = sessionId,
                        PlayerId = playerId,
                        Die1 = die1,
                        Die2 = die2
                    });

        public Task DoublingCubeAccepted(
            Guid sessionId,
            Guid acceptingPlayerId,
            int cubeValue)
            => _hub.Clients
                .Group(sessionId.ToString())
                .SendAsync("DoublingCubeAccepted", new
                {
                    SessionId = sessionId,
                    AcceptingPlayerId = acceptingPlayerId,
                    CubeValue = cubeValue
                });

        public Task DoublingCubeOffered(
            Guid sessionId,
            Guid playerId,
            int cubeValue)
            => _hub.Clients
            .Group(sessionId.ToString())
            .SendAsync("DoublingCubeOffered", new
            {
                SessionId = sessionId,
                PlayerId = playerId,
                CubeValue = cubeValue
            });

        public Task GameFinished(
            Guid sessionId,
            Guid winnerPlayerId,
            GameFinishReason reason,
            GameOutcome outcome)
            => _hub.Clients
            .Group(sessionId.ToString())
            .SendAsync("GameFinished", new
            {
                SessionId = sessionId,
                WinnerPlayerId = winnerPlayerId,
                Reason = reason.ToString(),
                Outcome = outcome
            });

        public Task GameStarted(
            Guid sessionId,
            Guid startingPlayerId)
            => _hub.Clients
                .Group(sessionId.ToString())
                .SendAsync("GameStarted", new
                {
                    StartingPlayerId = startingPlayerId
                });

        public Task PlayerDisconnected(
            Guid sessionId,
            Guid playerId,
            DateTimeOffset disconnectedAt)
            => _hub.Clients
                .Group(sessionId.ToString())
                .SendAsync("PlayerDisconnected", new
                {
                    PlayerId = playerId,
                    DisconnectedAt = disconnectedAt
                });


        public Task PlayerReconnected(
            Guid sessionId,
            Guid playerId,
            DateTimeOffset reconnectedAt)
            => _hub.Clients
                .Group(sessionId.ToString())
                .SendAsync("PlayerReconnected", new
                {
                    PlayerId = playerId,
                    ReconnectedAt = reconnectedAt
                });


        public Task PlayerTimeoutExpired(
            Guid sessionId,
            Guid timedOutPlayerId,
            Guid? winnerPlayerId)
            => _hub.Clients
                .Group(sessionId.ToString())
                .SendAsync("PlayerTimeoutExpired", new
                {
                    TimedOutPlayerId = timedOutPlayerId,
                    WinnerPlayerId = winnerPlayerId
                });
        

        public Task StartingPlayerDetermined(
            Guid sessionId,
            IEnumerable<(Guid PlayerId, int Roll)> rolls,
            Guid startingPlayerId)
            => _hub.Clients.Group(sessionId.ToString())
                .SendAsync("StartingPlayerDetermined", new
                {
                    Rolls = rolls.Select(r => new
                    {
                        r.PlayerId,
                        r.Roll
                    }),
                    StartingPlayerId = startingPlayerId
                });
    }
}
