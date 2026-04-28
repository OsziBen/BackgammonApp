using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Authorization
{
    public class GroupRoleRequirement : IAuthorizationRequirement
    {
        public string[] AllowedRoles { get; }

        public GroupRoleRequirement(params string[] allowedRoles)
        {
            AllowedRoles = allowedRoles;
        }
    }
}
