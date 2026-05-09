namespace Common.Enums.TournamentParticipant
{
    public enum TournamentParticipantStatus
    {
        Pending = 0,
        Active = 1,
        Withdrawn = 2,
        Disqualified = 3,   // for breaking rules only, used during the actual tournament (not MVP)
        Standby = 4,
        Removed = 5,
    }
}
