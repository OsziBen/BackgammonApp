using Application.Groups.Responses;
using Common.Constants;
using Domain.Group;
using Domain.GroupMembership;

namespace Application.Groups.Helpers
{
    public static class GroupResponseMapper
    {
        // membership based mapping
        public static GroupBaseResponse ToBaseResponse(
            Group group,
            GroupMembership? membership,
            bool hasPendingRequest)
        {
            string? groupUserState = null;

            if (membership != null)
            {
                groupUserState = membership.GroupRoles
                    .FirstOrDefault(r => r.IsActive)?
                    .GroupRole
                    .SystemName;
            }
            else if (hasPendingRequest)
            {
                groupUserState = GroupUserStates.Pending;
            }

            return BuildResponse(group, groupUserState);
        }

        // explicit state mapping
        public static GroupBaseResponse ToBaseResponse(
            Group group,
            string? groupUserState)
        {
            return BuildResponse(group, groupUserState);
        }

        private static GroupBaseResponse BuildResponse(
            Group group,
            string? groupUserState)
        {
            return new GroupBaseResponse
            {
                Id = group.Id,
                CreatorName = group.Creator.UserName,
                Name = group.Name,
                Description = group.Description,
                Visibility = group.Visibility.ToString(),
                JoinPolicy = group.JoinPolicy.ToString(),
                SizePreset = group.SizePreset.ToString(),
                MaxMembers = group.MaxMembers,
                MaxModerators = group.MaxModerators,
                GroupUserState = groupUserState,
                CreatedAt = group.CreatedAt,
            };
        }
    }
}