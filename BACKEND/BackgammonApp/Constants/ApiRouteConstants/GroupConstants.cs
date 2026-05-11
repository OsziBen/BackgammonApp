namespace WebAPI.Constants.ApiRouteConstants
{
    public static class GroupConstants
    {
        public const string Base = "groups";
        public const string ById = "{groupId:guid}";
        public const string Join = "{groupId:guid}/join";
        public const string Requests = "{groupId:guid}/requests";
        public const string ApproveJoinRequest = "{groupId:guid}/requests/{requestId:guid}/approve";
        public const string RejectJoinRequest = "{groupId:guid}/requests/{requestId:guid}/reject";
        public const string Leave = "{groupId:guid}/leave";
        public const string AllGroupMembers = "{groupId:guid}/members";
        public const string GroupMember = "{groupId:guid}/members/{userId:guid}";
        public const string AddMember = "{groupId:guid}/members/{string:userName}";
        public const string AllGroupModerators = "{groupId:guid}/moderators";
        public const string GroupModerator = "{groupId:guid}/moderators/{userId:guid}";
    }
}
