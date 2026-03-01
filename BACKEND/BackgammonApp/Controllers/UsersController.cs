using Application.Users.Commands.RegisterUser;
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
    [Route(UserConstants.Base)]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(UserConstants.Register)]
        [AllowAnonymous]
        public async Task<ActionResult<TokenResponse>> RegisterAsync(
            [FromBody] RegistrationRequest request,
            CancellationToken cancellationToken)
        {
            var command = new RegisterUserCommand(
                FirstName: request.FirstName,
                LastName: request.LastName,
                UserName: request.UserName,
                EmailAddress: request.EmailAddress,
                Password: request.Password,
                DateOfBirth: request.DateOfBirth);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
