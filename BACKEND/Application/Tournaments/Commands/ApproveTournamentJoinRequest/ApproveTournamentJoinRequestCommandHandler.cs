using Application.Interfaces.Repository;
using Application.Interfaces.Repository.Tournament;
using Application.Interfaces.Repository.TournamentParticipant;
using Application.Shared;
using Application.Shared.Time;
using Common.Enums;
using Common.Enums.Group;
using Common.Enums.TournamentParticipant;
using Common.Exceptions;
using Domain.Tournament;
using Domain.TournamentJoinRequest;
using Domain.TournamentParticipant;
using MediatR;

namespace Application.Tournaments.Commands.ApproveTournamentJoinRequest
{
    public class ApproveTournamentJoinRequestCommandHandler : IRequestHandler<ApproveTournamentJoinRequestCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITournamentParticipantReadRepository _tournamentParticipantReadRepository;
        private readonly ITournamentReadRepository _tournamentReadRepository;

        public ApproveTournamentJoinRequestCommandHandler(
            IUnitOfWork uow,
            IDateTimeProvider dateTimeProvider,
            ITournamentParticipantReadRepository tournamentParticipantReadRepository,
            ITournamentReadRepository tournamentReadRepository)
        {
            _uow = uow;
            _dateTimeProvider = dateTimeProvider;
            _tournamentParticipantReadRepository = tournamentParticipantReadRepository;
            _tournamentReadRepository = tournamentReadRepository;
        }

        public async Task<Unit> Handle(ApproveTournamentJoinRequestCommand request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;

            var joinRequest = await _uow.TournamentJoinRequestsWrite
                .GetByIdAsync(request.TournamentId, cancellationToken)
                .GetOrThrowAsync(nameof(TournamentJoinRequest), request.TournamentId);

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

            if (await _tournamentParticipantReadRepository.ExistsAsync(request.UserId, request.TournamentId, cancellationToken))
            {
                throw new BusinessRuleException(
                    FunctionCode.UserAlreadyActiveParticipant,
                    "User already a participant.");
            }

            var tournament = await _tournamentReadRepository
                .GetByIdAsync(request.TournamentId, cancellationToken)
                .GetOrThrowAsync(nameof(Tournament), request.TournamentId);

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
                UserId = request.UserId,
                Status = TournamentParticipantStatus.Active,
                DisplayName = joinRequest.User.UserName,
                Email = joinRequest.User.EmailAddress,
                Notes = null,
                CreatedAt = now,
                LastUpdatedAt = now,
            };

            joinRequest.Status = JoinRequestStatus.Approved;
            joinRequest.ReviewedAt = now;
            joinRequest.ReviewedByUserId = request.UserId;

            await _uow.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
