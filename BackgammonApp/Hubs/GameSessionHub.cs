using Application.GameSessions.Commands.AcceptDoublingCube;
using Application.GameSessions.Commands.DeclineDoublingCube;
using Application.GameSessions.Commands.JoinGameSession;
using Application.GameSessions.Commands.MoveCheckers;
using Application.GameSessions.Commands.OfferDoublingCube;
using Application.GameSessions.Commands.PlayerDisconnected;
using Application.GameSessions.Commands.PlayerForfeit;
using Application.GameSessions.Commands.PlayerReconnected;
using Application.GameSessions.Commands.RollDice;
using Application.GameSessions.Requests;
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

        public GameSessionHub(
            IMediator mediator,
            IConnectionMapping connections)
        {
            _mediator = mediator;
            _connections = connections;
        }

        public async Task JoinSession(string sessionCode)
        {
            var playerId = this.GetCurrentPlayerId(_connections);

            var result = await _mediator.Send(
                new JoinGameSessionCommand(
                    sessionCode,
                    playerId,
                    Context.ConnectionId
                ));

            _connections.Add(Context.ConnectionId, result.PlayerId);

            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                result.SessionId.ToString());

            await Clients.Caller.SendAsync("SessionJoined", result);

            if (result.IsRejoin)
            {
                await _mediator.Send(
                    new PlayerReconnectedCommand(result.PlayerId));
            }
            else
            {
                await Clients
                    .GroupExcept(result.SessionId.ToString(), Context.ConnectionId)
                    .SendAsync("PlayerJoined", new
                    {
                        result.PlayerId,
                        result.PlayerColor
                    });
            }
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
            var playerId = this.GetCurrentPlayerId(_connections);

            await _mediator.Send(
                new RollDiceCommand(sessionId, playerId));
        }

        public async Task OfferDoublingCube(Guid sessionId)
        {
            var playerId = this.GetCurrentPlayerId(_connections);

            await _mediator.Send(
                new OfferDoublingCubeCommand(sessionId, playerId));
        }

        public async Task AcceptDoublingCube(Guid sessionId)
        {
            var playerId = this.GetCurrentPlayerId(_connections);

            await _mediator.Send(
                new AcceptDoublingCubeCommand(sessionId, playerId));
        }

        public async Task DeclineDoublingCube(Guid sessionId)
        {
            var playerId = this.GetCurrentPlayerId(_connections);

            await _mediator.Send(
                new DeclineDoublingCubeCommand(sessionId, playerId));
        }

        public async Task MoveCheckers(
            Guid sessionId,
            IReadOnlyList<MoveDto> moves)
        {
            var playerId = this.GetCurrentPlayerId(_connections);

            await _mediator.Send(
                new MoveCheckersCommand(sessionId, playerId, moves));
        }

        public async Task Forfeit(Guid sessionId)
        {
            var playerId = this.GetCurrentPlayerId(_connections);

            await _mediator.Send(
                new PlayerForfeitCommand(sessionId, playerId));
        }
    }
}
