using FluentValidation;

namespace Application.Tournaments.Commands.RejectTournamentJoinRequest.Validators
{
    public class RejectTournamentJoinRequestCommandValidator : AbstractValidator<RejectTournamentJoinRequestCommand>
    {
        public RejectTournamentJoinRequestCommandValidator()
        {
            RuleFor(x => x.TournamentId)
               .NotEmpty()
               .WithMessage("Tournament ID is required.");

            RuleFor(x => x.UserId)
               .NotEmpty()
               .WithMessage("User ID is required.");

            RuleFor(x => x.RequestId)
               .NotEmpty()
               .WithMessage("Request ID is required.");
        }
    }
}
