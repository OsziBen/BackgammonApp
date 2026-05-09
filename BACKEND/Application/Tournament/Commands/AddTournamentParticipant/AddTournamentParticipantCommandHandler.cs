using Application.Interfaces.Repository;
using Application.Interfaces.Repository.Tournament;
using Application.Interfaces.Repository.TournamentParticipant;
using Application.Interfaces.Repository.User;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Enums.Tournament;
using Common.Enums.TournamentParticipant;
using Common.Exceptions;
using Domain.TournamentParticipant;
using Domain.User;
using MediatR;

namespace Application.Tournament.Commands.AddTournamentParticipant
{
    public class AddTournamentParticipantCommandHandler : IRequestHandler<AddTournamentParticipantCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITournamentReadRepository _tournamentReadRepository;
        private readonly ITournamentParticipantReadRepository _tournamentParticipantReadRepository;
        private readonly IUserReadRepository _userReadRepository;


        public AddTournamentParticipantCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider,
            ITournamentReadRepository tournamentReadRepository,
            ITournamentParticipantReadRepository tournamentParticipantReadRepository,
            IUserReadRepository userReadRepository)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
            _tournamentReadRepository = tournamentReadRepository;
            _tournamentParticipantReadRepository = tournamentParticipantReadRepository;
            _userReadRepository = userReadRepository;
        }

        public async Task<Unit> Handle(AddTournamentParticipantCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var tournament = await _tournamentReadRepository
                .GetByIdAsync(request.TournamentId, cancellationToken)
                .GetOrThrowAsync(nameof(Tournament), request.TournamentId);

            if (tournament.Visibility != TournamentVisibility.Private)
            {
                throw new BusinessRuleException(
                    FunctionCode.CannotAddParticipantDirectlyInPublicTournament,
                    "Direct add is only allowed for private tournaments.");
            }

            var user = await _userReadRepository
                .GetByUserNameAsync(request.UserName, cancellationToken)
                .GetOrThrowAsync(nameof(User), request.UserName);

            if (await _tournamentParticipantReadRepository.ExistsAsync(user.Id, request.TournamentId, cancellationToken))
            {
                throw new BusinessRuleException(
                    FunctionCode.UserAlreadyActiveParticipant,
                    "User already a participant.");
            }

            if (tournament.Participants.Count >= tournament.MaxParticipants)
            {
                throw new BusinessRuleException(
                    FunctionCode.TournamentReachedMaxParticipantsLimit,
                    "Tournament has reached its limit of participants.");
            }

            if (now >= tournament.Deadline)
            {
                throw new BusinessRuleException(
                    FunctionCode.JoinDeadlinePassed,
                    "The deadline for joining this tournament has passed.");
            }

            var participant = new TournamentParticipant
            {
                TournamentId = request.TournamentId,
                UserId = user.Id,
                Status = TournamentParticipantStatus.Active,
                DisplayName = user.UserName,
                Email = user.EmailAddress,
                Notes = null,
                CreatedAt = now,
                LastUpdatedAt = now,
            };

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
