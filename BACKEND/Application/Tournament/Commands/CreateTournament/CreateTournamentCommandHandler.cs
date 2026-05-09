using Application.Interfaces.Repository;
using Application.Interfaces.Repository.Tournament;
using Application.Interfaces.Repository.User;
using Application.Shared;
using Application.Shared.Time;
using Application.Tournament.Responses;
using Common.Enums;
using Common.Enums.Tournament;
using Common.Exceptions;
using Domain.User;
using MediatR;

namespace Application.Tournament.Commands.CreateTournament
{
    public class CreateTournamentCommandHandler : IRequestHandler<CreateTournamentCommand, TournamentBaseResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ITournamentReadRepository _tournamentReadRepository;

        public CreateTournamentCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider,
            IUserReadRepository userReadRepository,
            ITournamentReadRepository tournamentReadRepository)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
            _userReadRepository = userReadRepository;
            _tournamentReadRepository = tournamentReadRepository;
        }

        public async Task<TournamentBaseResponse> Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;
            var normalizedName = request.Name.Trim();

            var user = await _userReadRepository
                .GetByIdAsync(request.UserId, cancellationToken)
                .GetOrThrowAsync(nameof(User), request.UserId);

            var hasTournamentWithSameName = await _tournamentReadRepository.ExistsByNameAsync(request.Name, cancellationToken);

            if (hasTournamentWithSameName)
            {
                throw new BusinessRuleException(
                    FunctionCode.TournamentWithTournamentNameAlreadyExists,
                    $"Tournament with name {normalizedName} already exists.");
            }

            Domain.Tournament.Tournament tournament = new()
            {
                Name = normalizedName,
                Description = request.Description,
                Type = request.Type,
                Visibility = request.Visibility,
                Status = TournamentStatus.RegistrationOpen,
                MaxParticipants = request.MaxParticipants,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Deadline = request.Deadline,
                OrganizerUserId = user.Id,
                RulesTemplateId = request.RulesTemplateId,
                IsDeleted = false,
                CreatedAt = now,
                LastUpdatedAt = now,
                FinishedAt = null,
                DeletedAt = null
            };

            await _uow.TournamentsWrite.AddAsync(tournament, cancellationToken);

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
                Deadline= tournament.Deadline,
                OrganizerUserName = user.UserName,
            };
        }
    }
}
