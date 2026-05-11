namespace Application.Tournaments.Responses
{
    public class TournamentParticipantsResponse
    {
        public List<TournamentParticipantBaseResponse> Participants { get; set; } = [];
        public int? MaxParticipantsNumber { get; set; }
        public int CurrentParticipantsNumber { get; set; }
    }
}
