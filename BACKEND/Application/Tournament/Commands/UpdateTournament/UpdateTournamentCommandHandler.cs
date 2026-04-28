using Application.Interfaces.Repository;
using Application.Shared.Time;
using MediatR;

namespace Application.Tournament.Commands.UpdateTournament
{
    public class UpdateTournamentCommandHandler : IRequestHandler<UpdateTournamentCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdateTournamentCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task<Unit> Handle(UpdateTournamentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
