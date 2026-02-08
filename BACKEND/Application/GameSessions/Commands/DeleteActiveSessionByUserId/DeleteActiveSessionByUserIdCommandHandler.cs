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

        public DeleteActiveSessionByUserIdCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider timeProvider)
        {
            _uow = uow;
            _timeProvider = timeProvider;
        }

        public async Task<Unit> Handle(DeleteActiveSessionByUserIdCommand request, CancellationToken cancellationToken)
        {
            var session = await _uow.GameSessionsWrite.GetByIdAsync(request.SessionId);

            if (session == null)
            {
                throw new NotFoundException(FunctionCode.ResourceNotFound, "Session not found");
            }

            session.IsDeleted = true;
            session.DeletedAt = _timeProvider.UtcNow;

            await _uow.CommitAsync();

            return Unit.Value;
        }
    }
}
