using Application.Interfaces.Repository;
using Application.Interfaces.Repository.Group;
using Application.Shared;
using Application.Shared.Time;
using Domain.Group;
using MediatR;

namespace Application.Groups.Commands.DeleteGroup
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeleteGroupCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var group = await _uow.GroupsWrite
                .GetByIdAsync(request.GroupId, cancellationToken)
                .GetOrThrowAsync(nameof(Group), request.GroupId);

            group.IsDeleted = true;
            group.DeletedAt = now;
            group.LastUpdatedAt = now;

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
