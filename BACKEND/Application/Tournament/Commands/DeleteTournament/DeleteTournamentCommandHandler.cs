using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Exceptions;
using Domain.Tournament;
using MediatR;

namespace Application.Tournament.Commands.DeleteTournament
{
    public class DeleteTournamentCommandHandler : IRequestHandler<DeleteTournamentCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeleteTournamentCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(DeleteTournamentCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var tournament = await _uow.TournamentsWrite
                .GetByIdAsync(request.TournamentId, cancellationToken)
                .GetOrThrowAsync(nameof(Tournament), request.TournamentId);

            if (tournament.OrganizerUserId != request.UserId)
            {
                throw new BusinessRuleException(
                    FunctionCode.MissingPermission,
                    $"Tournament can only be deleted by the organiser.");
            }

            tournament.IsDeleted = true;
            tournament.DeletedAt = now;
            tournament.LastUpdatedAt = now;

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
