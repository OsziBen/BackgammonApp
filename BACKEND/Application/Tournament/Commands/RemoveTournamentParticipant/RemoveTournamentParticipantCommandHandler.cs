using Application.Interfaces.Repository;
using Application.Interfaces.Repository.Tournament;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Exceptions;
using Domain.Tournament;
using Domain.TournamentParticipant;
using MediatR;

namespace Application.Tournament.Commands.RemoveTournamentParticipant
{
    public class RemoveTournamentParticipantCommandHandler : IRequestHandler<RemoveTournamentParticipantCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITournamentReadRepository _tournamentReadRepository;

        public RemoveTournamentParticipantCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider,
            ITournamentReadRepository tournamentReadRepository)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
            _tournamentReadRepository = tournamentReadRepository;
        }

        public async Task<Unit> Handle(RemoveTournamentParticipantCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var participant = await _uow.TournamentParticipantsWrite
                .GetAsync(request.UserId, request.TournamentId, cancellationToken)
                .GetOrThrowAsync(nameof(TournamentParticipant), request.TournamentId);

            var tournament = await _tournamentReadRepository
                .GetByIdAsync(request.TournamentId, cancellationToken)
                .GetOrThrowAsync(nameof(Tournament), request.TournamentId);

            if (request.UserId == tournament.OrganizerUserId)
            {
                throw new BusinessRuleException(
                    FunctionCode.CannotRemoveTournamentOrganizer,
                    "Cannot remove the organizer of the tournament.");
            }

            participant.Status = Common.Enums.TournamentParticipant.TournamentParticipantStatus.Removed;
            participant.DeletedAt = now;
            participant.LastUpdatedAt = now;

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
