using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Application.Tournament.Responses;
using Common.Enums;
using Common.Exceptions;
using Domain.Tournament;
using MediatR;

namespace Application.Tournament.Commands.UpdateTournament
{
    public class UpdateTournamentCommandHandler : IRequestHandler<UpdateTournamentCommand, TournamentBaseResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdateTournamentCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
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
            tournament.Description = request.Description.Trim();
            tournament.Type = request.Type;
            tournament.Visibility = request.Visibility;
            tournament.Status = request.Status;
            tournament.MaxParticipants = request.MaxParticipants;
            tournament.StartDate = request.StartDate;
            tournament.EndDate = request.EndDate;
            tournament.Deadline = request.Deadline;
            tournament.RulesTemplateId = request.RulesTemplateId;

            tournament.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _uow.CommitAsync(cancellationToken);

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
                OrganizerUserName = tournament.OrganizerUser.UserName
            };
        }
    }
}
