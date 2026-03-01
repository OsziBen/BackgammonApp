using Application.Interfaces.Common;
using Application.Interfaces.Repository;
using Application.Shared.Time;
using Common.Enums;
using Common.Exceptions;
using MediatR;

namespace Application.GameSessions.Commands.DeleteActiveSessionByUserId
{
    public class DeleteActiveSessionByUserIdCommandHandler : IRequestHandler<DeleteActiveSessionByUserIdCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _timeProvider;
        private readonly ICurrentUser _currentUser;

        public DeleteActiveSessionByUserIdCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider timeProvider,
            ICurrentUser currentUser)
        {
            _uow = uow;
            _timeProvider = timeProvider;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(DeleteActiveSessionByUserIdCommand request, CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite.GetByIdAsync(request.SessionId, cancellationToken);

            if (session == null)
            {
                throw new NotFoundException(FunctionCode.ResourceNotFound, "Session not found");
            }

            if (session.CreatedByUserId != request.UserId)
            {
                throw new ForbiddenException(FunctionCode.AccessDenied, "You have no right to do this.");
            }

            session.MarkDeleted(_timeProvider.UtcNow);

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
