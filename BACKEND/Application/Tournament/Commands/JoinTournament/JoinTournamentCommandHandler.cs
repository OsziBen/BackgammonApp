using Application.Interfaces.Repository;
using Application.Interfaces.Repository.Tournament;
using Application.Interfaces.Repository.TournamentJoinRequest;
using Application.Interfaces.Repository.TournamentParticipant;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Enums.Group;
using Common.Enums.Tournament;
using Common.Exceptions;
using Domain.TournamentJoinRequest;
using MediatR;

namespace Application.Tournament.Commands.JoinTournament
{
    public class JoinTournamentCommandHandler : IRequestHandler<JoinTournamentCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITournamentJoinRequestReadRepository _tournamentJoinRequestReadRepository;
        private readonly ITournamentReadRepository _tournamentReadRepository;
        private readonly ITournamentParticipantReadRepository _tournamentParticipantReadRepository;

        public JoinTournamentCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider,
            ITournamentJoinRequestReadRepository tournamentJoinRequestReadRepository,
            ITournamentReadRepository tournamentReadRepository,
            ITournamentParticipantReadRepository tournamentParticipantReadRepository)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
            _tournamentJoinRequestReadRepository = tournamentJoinRequestReadRepository;
            _tournamentReadRepository = tournamentReadRepository;
            _tournamentParticipantReadRepository = tournamentParticipantReadRepository;
        }

        public async Task<Unit> Handle(JoinTournamentCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var tournament = await _tournamentReadRepository
                .GetByIdAsync(request.TournamentId, cancellationToken)
                .GetOrThrowAsync(nameof(Tournament), request.TournamentId);

            if (tournament.Visibility == TournamentVisibility.Private)
            {
                throw new BusinessRuleException(
                    FunctionCode.CannotJoinPrivateTournament,
                    "Cannot join a private tournament.");
            }

            if (await _tournamentParticipantReadRepository.ExistsAsync(request.UserId, request.TournamentId, cancellationToken))
            {
                throw new BusinessRuleException(
                    FunctionCode.UserAlreadyActiveParticipant,
                    "User already a participant.");
            }

            if (await _tournamentJoinRequestReadRepository.HasPendingRequestAsync(request.UserId, request.TournamentId, cancellationToken))
            {
                throw new BusinessRuleException(
                    FunctionCode.JoinAlreadyRequested,
                    "Join already requested.");
            }

            if (tournament.Participants.Count >= tournament.MaxParticipants)
            {
                throw new BusinessRuleException(
                    FunctionCode.TournamentReachedMaxParticipantsLimit,
                    "Tournament has reached its limit of participants.");
            }

            await _uow.TournamentJoinRequestsWrite.AddAsync(new TournamentJoinRequest
            {
                UserId = request.UserId,
                TournamentId = request.TournamentId,
                Status = JoinRequestStatus.Pending,
                CreatedAt = now,
            }, cancellationToken);

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
