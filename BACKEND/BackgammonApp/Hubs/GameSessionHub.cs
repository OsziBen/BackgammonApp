using Application.GameSessions.Commands.AcceptDoublingCube;
using Application.GameSessions.Commands.DeclineDoublingCube;
using Application.GameSessions.Commands.JoinGameSession;
using Application.GameSessions.Commands.MoveCheckers;
using Application.GameSessions.Commands.OfferDoublingCube;
using Application.GameSessions.Commands.PlayerDisconnected;
using Application.GameSessions.Commands.PlayerForfeit;
using Application.GameSessions.Commands.RollDice;
using Application.GameSessions.Commands.TryStartGameSession;
using Application.GameSessions.Requests;
using Application.Interfaces.Common;
using Application.Interfaces.Repository.GameSession;
using Application.Realtime.Connections;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Extensions;

namespace WebAPI.Hubs
{
    public class GameSessionHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly IConnectionMapping _connections;
        private readonly ICurrentUser _currentUser;
        private readonly IGameSessionReadRepository _gameSessionReadRepository;

        public GameSessionHub(
            IMediator mediator,
            IConnectionMapping connections,
            ICurrentUser currentUser,
            IGameSessionReadRepository gameSessionReadRepository)
        {
            _mediator = mediator;
            _connections = connections;
            _currentUser = currentUser;
            _gameSessionReadRepository = gameSessionReadRepository;
        }

        public async Task JoinSession(string sessionCode)
        {
            if (!_currentUser.IsAuthenticated)
            {
                throw new HubException("Unauthorized");
            }

            var session = await _gameSessionReadRepository
                .GetBySessionCodeAsync(sessionCode, includePlayers: false, cancellationToken: CancellationToken.None);

            if (session == null) {
                throw new HubException("Session not found");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId,
                session.Id.ToString());

            var result = await _mediator.Send(
                new JoinGameSessionCommand(sessionCode, _currentUser.UserId), Context.ConnectionAborted);

            _connections.Remove(Context.ConnectionId);
            _connections.Add(Context.ConnectionId, result.Player.Id);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_connections.TryGet(Context.ConnectionId, out var playerId))
            {
                await _mediator.Send(
                    new PlayerDisconnectedCommand(playerId));

                _connections.Remove(Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task RollDice(Guid sessionId)
        {
            var userId = this.GetCurrentPlayerId(_connections);

            await _mediator.Send(
                new RollDiceCommand(sessionId, userId));
        }

        public async Task OfferDoublingCube(Guid sessionId)
        {
            var userId = this.GetCurrentPlayerId(_connections);

            await _mediator.Send(
                new OfferDoublingCubeCommand(sessionId, userId));
        }

        public async Task AcceptDoublingCube(Guid sessionId)
        {
            var userId = this.GetCurrentPlayerId(_connections);

            await _mediator.Send(
                new AcceptDoublingCubeCommand(sessionId, userId));
        }

        public async Task DeclineDoublingCube(Guid sessionId)
        {
            var userId = this.GetCurrentPlayerId(_connections);

            await _mediator.Send(
                new DeclineDoublingCubeCommand(sessionId, userId));
        }

        public async Task MoveCheckers(
            Guid sessionId,
            IReadOnlyList<MoveDto> moves)
        {
            var userId = this.GetCurrentPlayerId(_connections);

            await _mediator.Send(
                new MoveCheckersCommand(sessionId, userId, moves));
        }

        public async Task Forfeit(Guid sessionId)
        {
            var userId = this.GetCurrentPlayerId(_connections);

            await _mediator.Send(
                new PlayerForfeitCommand(sessionId, userId));
        }
    }
}
