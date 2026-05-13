using Application.Interfaces.Repository;
using Application.Interfaces.Repository.RulesTemplate;
using Application.RulesTemplate.Responses;
using Application.Shared;
using Application.Shared.Time;
using Application.Tournaments.Responses;
using Common.Constants;
using Common.Enums;
using Common.Exceptions;
using Domain.RulesTemplate;
using Domain.Tournament;
using MediatR;

namespace Application.Tournaments.Commands.UpdateTournament
{
    public class UpdateTournamentCommandHandler : IRequestHandler<UpdateTournamentCommand, TournamentBaseResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRulesTemplateReadRepository _rulesTemplateReadRepository;

        public UpdateTournamentCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider,
            IRulesTemplateReadRepository rulesTemplateReadRepository)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
            _rulesTemplateReadRepository = rulesTemplateReadRepository;
        }

        public async Task<TournamentBaseResponse> Handle(UpdateTournamentCommand request, CancellationToken cancellationToken)
        {
            var tournament = await _uow.TournamentsWrite
                .GetByIdAsync(request.TournamentId, cancellationToken)
                .GetOrThrowAsync(nameof(Tournament), request.TournamentId);

            if (request.MaxParticipants < tournament.Participants.Count)
            {
                throw new BusinessRuleException(
                    FunctionCode.MaxParicipantsCannotBeLessThanCurrentNumberOfParticipants,
                    "Max paricipants cannot be less than current Number of participants");
            }

            tournament.Name = request.Name.Trim();
            tournament.Description = request.Description?.Trim();
            tournament.Type = request.Type;
            tournament.Visibility = request.Visibility;
            tournament.Status = request.Status;
            tournament.MaxParticipants = request.MaxParticipants;
            tournament.StartDate = request.StartDate.ToUniversalTime();
            tournament.EndDate = request.EndDate.ToUniversalTime();
            tournament.Deadline = request.Deadline.ToUniversalTime();
            tournament.RulesTemplateId = request.RulesTemplateId;

            tournament.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _uow.CommitAsync(cancellationToken);

            var template = await _rulesTemplateReadRepository
                .GetByIdAsync(request.RulesTemplateId, cancellationToken)
                .GetOrThrowAsync(nameof(RulesTemplate), request.RulesTemplateId);

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
                TournamentUserState = TournamentUserStates.Organizer
            };
        }
    }
}
