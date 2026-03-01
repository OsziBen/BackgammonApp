using Application.GameSessions.Commands.CreateGameSession;
using Application.GameSessions.Commands.DeleteActiveSessionByUserId;
using Application.GameSessions.Commands.GetActiveSessionByUserId;
using Application.GameSessions.Responses;
using Application.Interfaces.Common;
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
        private readonly ICurrentUser _currentUser;

        public GameSessionsController(
            IMediator mediator,
            ICurrentUser currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpPost]
        public async Task<ActionResult<CreateGameSessionResponse>> CreateSessionAsync(
            [FromBody] CreateGameSessionRequest request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
                return Unauthorized();

            var command = new CreateGameSessionCommand(
                userId, request.Settings);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet(GameSessionConstants.Active)]
        public async Task<ActionResult<GetActiveSessionByUserIdResponse?>> GetActiveSessionAsync(
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
                return Unauthorized();

            var command = new GetActiveSessionByUserIdCommand(userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpDelete(GameSessionConstants.ById)]
        public async Task<ActionResult> SoftDeleteSessionAsync(
            [FromRoute] Guid sessionId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
                return Unauthorized();

            var command = new DeleteActiveSessionByUserIdCommand(userId, sessionId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
