using Application.Groups.Responses;
using Application.Interfaces.Common;
using Application.Tournament.Responses;
using Application.Users.Commands.GetUserById;
using Application.Users.Commands.ListGroupJoinRequestsByUserId;
using Application.Users.Commands.ListTournamentJoinRequestsByUserId;
using Application.Users.Commands.ListUserGroups;
using Application.Users.Commands.ListUserTournaments;
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
        private readonly ICurrentUser _currentUser;

        public UsersController(
            IMediator mediator,
            ICurrentUser currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
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

        [HttpGet(UserConstants.Groups)]
        public async Task<ActionResult<List<GroupBaseResponse>>> GetAllGroupsAsync(
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new ListUserGroupsCommand(userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet(UserConstants.GroupJoinRequests)]
        public async Task<ActionResult<List<UserGroupJoinRequestResponse>>> GetAllGroupJoinRequestsAsync(
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new ListGroupJoinRequestsByUserIdCommand(userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet(UserConstants.Tournaments)]
        public async Task<ActionResult<List<TournamentBaseResponse>>> GetAllTournamentsAsync(
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new ListUserTournamentsCommand(userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet(UserConstants.TournamentJoinRequests)]
        public async Task<ActionResult<List<UserTournamentJoinRequestResponse>>> GetAllTournamentJoinRequestsAsync(
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new ListTournamentJoinRequestsByUserIdCommand(userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet(UserConstants.Me)]
        public async Task<ActionResult<UserProfileResponse>> GetByIdAsync(
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new GetUserByIdCommand(userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
