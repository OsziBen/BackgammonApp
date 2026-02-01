using Application.GameSessions.Commands.CreateGameSession;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constants.ApiRouteConstants;

namespace WebAPI.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route(GameSessionConstants.Base)]
    public class GameSessionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GameSessionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateSessionAsync(
            [FromBody] CreateGameSessionRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateGameSessionCommand(
                request.HostPlayerId, request.Settings);

            var sessionId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { sessionId }, sessionId);
        }

        [HttpGet(GameSessionConstants.ById)]
        public IActionResult GetById(Guid sessionId)
        {
            return Ok();
        }
    }
}
