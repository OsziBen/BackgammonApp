using Application.Realtime.Connections;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WebAPI.Extensions
{
    public static class HubExtensions
    {
        public static Guid GetCurrentPlayerId(
            this Hub hub,
            IConnectionMapping connections)
        {
            var claimValue = hub.Context.User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            if (Guid.TryParse(claimValue, out var playerId))
            {
                return playerId;
            }

            if (connections.TryGet(
                hub.Context.ConnectionId,
                out var mappedPlayerId))
            {
                return mappedPlayerId;
            }

            throw new HubException(
                "Caller is not associated with a player");
        }
    }
}
