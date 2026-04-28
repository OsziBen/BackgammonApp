using Application.Interfaces.Repository;
using Application.Shared.Time;
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

        public Task<Unit> Handle(DeleteTournamentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
