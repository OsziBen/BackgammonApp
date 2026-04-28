using Application.Interfaces.Repository;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Exceptions;
using Domain.GroupJoinRequest;
using MediatR;

namespace Application.Groups.Commands.RejectJoinRequest
{
    public class RejectJoinRequestCommandHandler : IRequestHandler<RejectJoinRequestCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RejectJoinRequestCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(RejectJoinRequestCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var joinRequest = await _uow.GroupJoinRequestsWrite
                .GetByIdAsync(request.RequestId, cancellationToken)
                .GetOrThrowAsync(nameof(GroupJoinRequest), request.RequestId);

            if (joinRequest.Status != JoinRequestStatus.Pending)
            {
                throw new BusinessRuleException(
                    FunctionCode.InvalidJoinRequestStatus,
                    "Join request is not in pending state.");
            }

            if (joinRequest.GroupId != request.GroupId)
            {
                throw new BusinessRuleException(
                    FunctionCode.GroupMismatch,
                    "Group IDs are not matching.");
            }

            joinRequest.Status = JoinRequestStatus.Rejected;
            joinRequest.ReviewedAt = now;
            joinRequest.ReviewedByUserId = request.UserId;

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
