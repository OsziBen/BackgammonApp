namespace WebAPI.Constants.ApiRouteConstants
{
    public static class TournamentConstants
    {
        public const string Base = "tournaments";
        public const string ById = "{tournamentId:guid}";
        public const string Join = "{tournamentId:guid}/join";
        public const string Requests = "{tournamentId:guid}/requests";
        public const string ApproveJoinRequest = "{tournamentId:guid}/requests/{requestId:guid}/approve";
        public const string RejectJoinRequest = "{tournamentId:guid}/requests/{requestId:guid}/reject";
        public const string Withdraw = "{tournamentId:guid}/withdraw";
        public const string TournamentParticipants = "{tournamentId:guid}/participants";
        public const string Participant = "{tournamentId:guid}/participants/{userId:guid}";
    }
}
