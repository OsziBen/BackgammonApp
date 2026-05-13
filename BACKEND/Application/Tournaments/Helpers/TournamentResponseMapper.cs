using Application.RulesTemplate.Responses;
using Application.Tournaments.Responses;
using Common.Constants;

namespace Application.Tournaments.Helpers
{
    public static class TournamentResponseMapper
    {
        public static TournamentBaseResponse ToBaseResponse(
            Domain.Tournament.Tournament tournament,
            Guid? userId,
            bool hasPendingRequest = false)
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
                else if (hasPendingRequest)
                {
                    state = TournamentUserStates.Pending;
                }
                else
                {
                    state = TournamentUserStates.None;
                }
            }

            return BuildResponse(tournament, state);
        }

        private static TournamentBaseResponse BuildResponse(
            Domain.Tournament.Tournament tournament,
            string? state)
        {
            var template = tournament.RulesTemplate;

            var templateResponse = new RulesTemplateResponse
            {
                Id = template.Id,
                Name = template.Name,
                Description = template.Description,
                AuthorName = template.Author?.UserName ?? "System",
                TargetScore = template.TargetScore,
                UseClock = template.UseClock,
                MatchTimePerPlayerInSeconds = template.MatchTimePerPlayerInSeconds,
                StartOfTurnDelayPerPlayerInSeconds = template.StartOfTurnDelayPerPlayerInSeconds,
                CrawfordRuleEnabled = template.CrawfordRuleEnabled,
                CreatedAt = template.CreatedAt,
            };

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
                RulesTemplate = templateResponse,
                OrganizerUserName = tournament.OrganizerUser.UserName,
                TournamentUserState = state
            };
        }
    }
}