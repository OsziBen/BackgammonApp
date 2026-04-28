using Application.Interfaces.Repository.GroupMembership;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Authorization
{
    public class GroupRoleHandler : AuthorizationHandler<GroupRoleRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGroupMembershipReadRepository _groupMembershipReadRepository;

        public GroupRoleHandler(
            IHttpContextAccessor httpContextAccessor,
            IGroupMembershipReadRepository groupMembershipReadRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _groupMembershipReadRepository = groupMembershipReadRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GroupRoleRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return;
            }

            var httpContext = _httpContextAccessor.HttpContext!;
            var groupIdStr = httpContext.Request.RouteValues["groupId"]?.ToString();

            if (!Guid.TryParse(groupIdStr, out var groupId))
            {
                return;
            }

            var role = await _groupMembershipReadRepository.GetUserRoleAsync(userId, groupId, CancellationToken.None);

            if (role == null)
            {
                return;
            }

            if (requirement.AllowedRoles.Contains(role))
            {
                context.Succeed(requirement);
            }
        }
    }
}
