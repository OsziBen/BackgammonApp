using Application.Users.Responses;

namespace Application.Groups.Responses
{
    public class GroupMembersResponse
    {
        public List<UserBaseResponse> Members { get; set; } = [];
        public int MaxModeratorNumber { get; set; }
        public int CurrentModeratorNumber { get; set; }
        public int MaxMemeberNumber { get; set; }
        public int CurrentMemberNumber { get; set; }
    }
}
