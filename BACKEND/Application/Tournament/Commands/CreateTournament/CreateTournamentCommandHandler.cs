using Application.Interfaces.Repository;
using Application.Shared.Time;
using MediatR;

namespace Application.Tournament.Commands.CreateTournament
{
    public class CreateTournamentCommandHandler : IRequestHandler<CreateTournamentCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateTournamentCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task<Unit> Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
