using Application.Tournaments.Responses;
using Common.Constants;

namespace Application.Tournaments.Helpers
{
    public static class TournamentResponseMapper
    {
        public static TournamentBaseResponse ToBaseResponse(
            Domain.Tournament.Tournament tournament,
            Guid? userId)
        {
            string? state = null;

            if (userId.HasValue)
            {
                if (tournament.OrganizerUserId == userId.Value)
                {
                    state = TournamentUserStates.Organizer;
                }
                else if (tournament.Participants.Any(p => p.UserId == userId.Value))
                {
                    state = TournamentUserStates.Participant;
                }
            }

            return BuildResponse(tournament, state);
        }

        private static TournamentBaseResponse BuildResponse(
            Domain.Tournament.Tournament tournament,
            string? state)
        {
            return new TournamentBaseResponse
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                Type = tournament.Type.ToString(),
                Visibility = tournament.Visibility.ToString(),
                Status = tournament.Status.ToString(),
                MaxParticipants = tournament.MaxParticipants,
                StartDate = tournament.StartDate,
                EndDate = tournament.EndDate,
                Deadline = tournament.Deadline,
                OrganizerUserName = tournament.OrganizerUser.UserName,
                TournamentUserState = state
            };
        }
    }
}