using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums.TournamentParticipant;
using Domain.TournamentParticipant;
using MediatR;

namespace Application.Tournament.Commands.WithdrawTournamentParticipation
{
    public class WithdrawTournamentParticipationCommandHandler : IRequestHandler<WithdrawTournamentParticipationCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;

        public WithdrawTournamentParticipationCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(WithdrawTournamentParticipationCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var participant = await _uow.TournamentParticipantsWrite
                .GetAsync(request.UserId, request.TournamentId, cancellationToken)
                .GetOrThrowAsync(nameof(TournamentParticipant), request.TournamentId);

            participant.Status = TournamentParticipantStatus.Withdrawn;
            participant.LastUpdatedAt = now;

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
