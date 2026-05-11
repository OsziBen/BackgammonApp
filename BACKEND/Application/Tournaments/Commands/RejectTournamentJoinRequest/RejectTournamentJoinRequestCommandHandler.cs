using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Enums.Group;
using Common.Exceptions;
using Domain.TournamentJoinRequest;
using MediatR;

namespace Application.Tournaments.Commands.RejectTournamentJoinRequest
{
    public class RejectTournamentJoinRequestCommandHandler : IRequestHandler<RejectTournamentJoinRequestCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RejectTournamentJoinRequestCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(RejectTournamentJoinRequestCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var joinRequest = await _uow.TournamentJoinRequestsWrite
                .GetByIdAsync(request.RequestId, cancellationToken)
                .GetOrThrowAsync(nameof(TournamentJoinRequest), request.RequestId);

            if (joinRequest.Status != JoinRequestStatus.Pending)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidJoinRequestStatus,
                    "Join request is not in pending state.");
            }

            if (joinRequest.TournamentId != request.TournamentId)
            {
                throw new BusinessRuleException(
                    FunctionCode.TournamentMismatch,
                    "Tournament IDs are not macthing.");
            }

            joinRequest.Status = JoinRequestStatus.Rejected;
            joinRequest.ReviewedAt = now;
            joinRequest.ReviewedByUserId = request.UserId;

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
