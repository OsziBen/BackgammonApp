using Application.GameSessions.Commands.JoinGameSession;
using Application.GameSessions.Commands.PlayerDisconnected;
using Application.GameSessions.Commands.PlayerReconnected;
using Application.Realtime.Connections;
using MediatR;
using Microsoft.AspNetCore.SignalR;

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

        public async Task JoinSession(string sessionCode, Guid userId)
        {
            var result = await _mediator.Send(
                new JoinGameSessionCommand(
                    sessionCode,
                    userId,
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
            if (_connections.TryGet(Context.ConnectionId, out var gamePlayerId))
            {
                await _mediator.Send(
                    new PlayerDisconnectedCommand(gamePlayerId));

                _connections.Remove(Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
