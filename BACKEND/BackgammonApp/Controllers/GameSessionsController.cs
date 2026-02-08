using Application.GameSessions.Commands.CreateGameSession;
using Application.GameSessions.Commands.DeleteActiveSessionByUserId;
using Application.GameSessions.Commands.GetActiveSessionByUserId;
using Application.GameSessions.Responses;
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
        public async Task<ActionResult<CreateGameSessionResponse>> CreateSessionAsync(
            [FromBody] CreateGameSessionRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateGameSessionCommand(     // todo: extension method to extract current player
                request.HostPlayerId, request.Settings);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet(GameSessionConstants.ActiveByUserId)]
        public async Task<ActionResult<GetActiveSessionByUserIdResponse?>> GetActiveSessionByUserId(
            [FromRoute] Guid userId,
            CancellationToken cancellationToken)
        {
            // TODO: extension method to extract current player
            var command = new GetActiveSessionByUserIdCommand(userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpDelete(GameSessionConstants.ById)]
        public async Task<ActionResult> SoftDeleteSessionAsync(
            [FromRoute] Guid sessionId,
            CancellationToken cancellationToken)
        {
            // TODO: extension method to extract current player
            var command = new DeleteActiveSessionByUserIdCommand(sessionId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
