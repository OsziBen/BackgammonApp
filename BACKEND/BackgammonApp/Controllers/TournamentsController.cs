using Application.Groups.Commands.CreateGroup;
using Application.Groups.Commands.GetAllGroups;
using Application.Interfaces.Common;
using Application.Tournament.Commands.CreateTournament;
using Application.Tournament.Commands.GetAllTournaments;
using Application.Tournament.Requests;
using Application.Tournament.Responses;
using Asp.Versioning;
using Common.Enums.Group;
using Common.Enums.Tournament;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constants.ApiRouteConstants;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route(TournamentConstants.Base)]
    public class TournamentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUser _currentUser;

        public TournamentsController(
            IMediator mediator,
            ICurrentUser currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpPost]
        public async Task<ActionResult<TournamentBaseResponse>> CreateTournamentAsync(
            [FromBody] CreateTournamentRequest request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new CreateTournamentCommand(
                userId,
                request.Name,
                request.Description,
                request.Type.ParseEnum<TournamentType>(),
                request.Visibility.ParseEnum<TournamentVisibility>(),
                request.MaxParticipants,
                request.RulesTemplateId,
                request.StartDate.ToUniversalTime(),
                request.EndDate.ToUniversalTime(),
                request.Deadline.ToUniversalTime());

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<TournamentBaseResponse>>> GetAllTournamentsAsync(
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new GetAllTournamentsCommand();

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
