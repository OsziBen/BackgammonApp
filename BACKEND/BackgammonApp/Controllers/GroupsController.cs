using Application.Groups.Commands.AddGroupMember;
using Application.Groups.Commands.ApproveGroupJoinRequest;
using Application.Groups.Commands.CreateGroup;
using Application.Groups.Commands.DeleteGroup;
using Application.Groups.Commands.DemoteModerator;
using Application.Groups.Commands.EditGroup;
using Application.Groups.Commands.GetAllPublicGroups;
using Application.Groups.Commands.GetGroupById;
using Application.Groups.Commands.JoinGroup;
using Application.Groups.Commands.LeaveGroup;
using Application.Groups.Commands.ListGroupJoinRequests;
using Application.Groups.Commands.ListGroupMembers;
using Application.Groups.Commands.PromoteGroupMemberToModerator;
using Application.Groups.Commands.RejectGroupJoinRequest;
using Application.Groups.Commands.RemoveGroupMember;
using Application.Groups.Requests;
using Application.Groups.Responses;
using Application.Interfaces.Common;
using Asp.Versioning;
using Common.Enums.Group;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Authorization;
using WebAPI.Constants.ApiRouteConstants;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route(GroupConstants.Base)]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUser _currentUser;

        public GroupsController(
            IMediator mediator,
            ICurrentUser currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpPost]
        public async Task<ActionResult<GroupBaseResponse>> CreateGroupAsync(
            [FromBody] CreateGroupRequest request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new CreateGroupCommand(
                userId,
                request.Name,
                request.Description,
                request.Visibility.ParseEnum<GroupVisibility>(),
                request.SizePreset.ParseEnum<GroupSizePreset>());

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<GroupBaseResponse>>> GetAllGroupsAsync(
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new GetAllPublicGroupsCommand(userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet(GroupConstants.ById)]
        public async Task<ActionResult<GroupBaseResponse>> GetGroupGyIdAsync(
            [FromRoute] Guid groupId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new GetGroupByIdCommand(groupId, userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPatch(GroupConstants.ById)]
        [Authorize(Policy = Policies.GroupOwner)]
        public async Task<ActionResult<GroupBaseResponse>> EditGroupAsync(
            [FromRoute] Guid groupId,
            [FromBody] EditGroupRequest request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new EditGroupCommand(
                groupId,
                request.Name,
                request.Description,
                request.Visibility.ParseEnum<GroupVisibility>(),
                request.SizePreset.ParseEnum<GroupSizePreset>());

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpDelete(GroupConstants.ById)]
        [Authorize(Policy = Policies.GroupOwner)]
        public async Task<ActionResult> SoftDeleteGroupAsync(
            [FromRoute] Guid groupId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new DeleteGroupCommand(groupId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost(GroupConstants.Join)]
        public async Task<ActionResult> JoinAsync(
            [FromRoute] Guid groupId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new JoinGroupCommand(groupId, userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost(GroupConstants.AllGroupMembers)]
        [Authorize(Policy = Policies.GroupModerator)]
        public async Task<ActionResult> AddMember(
            [FromRoute] Guid groupId,
            [FromBody] AddGroupMemberRequest request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new AddGroupMemberCommand(groupId, request.UserName, userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet(GroupConstants.Requests)]
        [Authorize(Policy = Policies.GroupModerator)]
        public async Task<ActionResult<List<GroupJoinRequestResponse>>> GetJoinRequestsAsync(
            [FromRoute] Guid groupId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new ListGroupJoinRequestsCommand(groupId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost(GroupConstants.ApproveJoinRequest)]
        [Authorize(Policy = Policies.GroupModerator)]
        public async Task<ActionResult> ApproveJoinRequestAsync(
            [FromRoute] Guid groupId,
            [FromRoute] Guid requestId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new ApproveGroupJoinRequestCommand(groupId, userId, requestId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost(GroupConstants.RejectJoinRequest)]
        [Authorize(Policy = Policies.GroupModerator)]
        public async Task<ActionResult> RejectJoinRequestAsync(
            [FromRoute] Guid groupId,
            [FromRoute] Guid requestId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new RejectGroupJoinRequestCommand(groupId, userId, requestId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet(GroupConstants.AllGroupMembers)]
        [Authorize(Policy = Policies.GroupMember)]
        public async Task<ActionResult<GroupMembersResponse>> GetAllMembersAsync(
            [FromRoute] Guid groupId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new ListGroupMembersCommand(groupId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost(GroupConstants.Leave)]
        public async Task<ActionResult> LeaveGroupAsync(
            [FromRoute] Guid groupId,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new LeaveGroupCommand(groupId, userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpDelete(GroupConstants.GroupMember)]
        [Authorize(Policy = Policies.GroupModerator)]
        public async Task<ActionResult> RemoveMemberAsync(
            [FromRoute] Guid groupId,
            [FromRoute] Guid userId,
            CancellationToken cancellationToken)
        {
            if (_currentUser.UserId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new RemoveGroupMemberCommand(groupId, userId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost(GroupConstants.GroupModerator)]
        [Authorize(Policy = Policies.GroupOwner)]
        public async Task<IActionResult> PromoteToModerator(
        Guid groupId,
        Guid userId,
        CancellationToken cancellationToken)
        {
            var currentUserId = _currentUser.UserId;

            if (currentUserId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new PromoteGroupMemberToModeratorCommand(groupId, userId, currentUserId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpDelete(GroupConstants.GroupModerator)]
        [Authorize(Policy = Policies.GroupOwner)]
        public async Task<IActionResult> DemoteModerator(
        Guid groupId,
        Guid userId,
        CancellationToken cancellationToken)
        {
            var currentUserId = _currentUser.UserId;

            if (currentUserId == Guid.Empty)
            {
                return Unauthorized();
            }

            var command = new DemoteModeratorCommand(groupId, userId, currentUserId);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
