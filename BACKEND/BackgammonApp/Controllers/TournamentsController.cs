using Application.Interfaces.Common;
using Application.Tournament.Commands.AddTournamentParticipant;
using Application.Tournament.Commands.ApproveTournamentJoinRequest;
using Application.Tournament.Commands.CreateTournament;
using Application.Tournament.Commands.DeleteTournament;
using Application.Tournament.Commands.GetAllTournaments;
using Application.Tournament.Commands.GetTournamentById;
using Application.Tournament.Commands.JoinTournament;
using Application.Tournament.Commands.ListTournamentJoinRequests;
using Application.Tournament.Commands.ListTournamentParticipants;
using Application.Tournament.Commands.RejectTournamentJoinRequest;
using Application.Tournament.Commands.RemoveTournamentParticipant;
using Application.Tournament.Commands.UpdateTournament;
using Application.Tournament.Commands.WithdrawTournamentParticipation;
using Application.Tournament.Requests;
using Application.Tournament.Responses;
using Asp.Versioning;
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

        [HttpGet(TournamentConstants.ById)]
        public async Task<ActionResult<TournamentBaseResponse>> GetByIdAsync(
            [FromRoute] Guid tournamentId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new GetTournamentByIdCommand(tournamentId, userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPatch(TournamentConstants.ById)]
        public async Task<ActionResult<TournamentBaseResponse>> UpdateTournamentAsync(
            [FromRoute] Guid tournamentId,
            [FromBody] UpdateTournamentRequest request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new UpdateTournamentCommand(
                tournamentId,
                request.Name,
                request.Description,
                request.Type.ParseEnum<TournamentType>(),
                request.Visibility.ParseEnum<TournamentVisibility>(),
                request.Status.ParseEnum<TournamentStatus>(),
                request.MaxParticipants,
                request.StartDate,
                request.EndDate,
                request.Deadline,
                request.RulesTemplateId);

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

        [HttpDelete(TournamentConstants.ById)]
        public async Task<ActionResult> SoftDeleteTournamentAsync(
            [FromRoute] Guid tournamentId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new DeleteTournamentCommand(tournamentId, userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost(TournamentConstants.Join)]
        public async Task<ActionResult> JoinTournamentAsync(
            [FromRoute] Guid tournamentId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new JoinTournamentCommand(tournamentId, userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost(TournamentConstants.ApproveJoinRequest)]
        public async Task<ActionResult> ApproveTournamentJoinRequestAsync(
            [FromRoute] Guid tournamentId,
            [FromRoute] Guid requestId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new ApproveTournamentJoinRequestCommand(tournamentId, userId, requestId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost(TournamentConstants.RejectJoinRequest)]
        public async Task<ActionResult> RejectTournamentJoinRequestAsync(
            [FromRoute] Guid tournamentId,
            [FromRoute] Guid requestId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new RejectTournamentJoinRequestCommand(tournamentId, userId, requestId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet(TournamentConstants.Requests)]
        public async Task<ActionResult<List<TournamentJoinRequestResponse>>> ListTournamentJoinRequestsAsync(
            [FromRoute] Guid tournamentId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new ListTournamentJoinRequestsCommand(tournamentId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet(TournamentConstants.AllTournamentParticipants)]
        public async Task<ActionResult<TournamentParticipantsResponse>> ListTournamentParticipantsAsync(
            [FromRoute] Guid tournamentId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new ListTournamentParticipantsCommand(tournamentId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }


        [HttpPost(TournamentConstants.TournamentParticipant)]
        public async Task<ActionResult> AddTournamentParticipant(
            [FromRoute] Guid tournamentId,
            [FromRoute] string userName,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new AddTournamentParticipantCommand(tournamentId, userName, userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpDelete(TournamentConstants.TournamentParticipant)]
        public async Task<ActionResult> RemoveTournamentParticipantAsync(
            [FromRoute] Guid tournamentId,
            [FromRoute] Guid userId,
            CancellationToken cancellationToken)
        {
            if (_currentUser.UserId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new RemoveTournamentParticipantCommand(tournamentId, userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost(TournamentConstants.Withdraw)]
        public async Task<ActionResult> WithdrawTournamentParticipationAsync(
            [FromRoute] Guid tournamentId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new WithdrawTournamentParticipationCommand(tournamentId, userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
