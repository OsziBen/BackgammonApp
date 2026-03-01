using Application.Authentication.Commands.Login;
using Application.Users.Responses;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constants.ApiRouteConstants;

namespace WebAPI.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route(AuthConstants.Base)]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(AuthConstants.Login)]
        [AllowAnonymous]
        public async Task<ActionResult<TokenResponse>> LoginAsync(
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.EmailAddress, request.Password);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
